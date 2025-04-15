using Microsoft.Playwright;
using TechTalk.SpecFlow;
using server.TestServices;
using Xunit;

namespace PlaywrightTests;

[Binding]
public class PlaywrigtTests
{
    private TestService _testService;
    
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;

    private string validChatLinkForLogin;
    private string chatId;

    [BeforeScenario]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new() { Headless = true, SlowMo = 200 });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
        _testService = new TestService();
    }

    [AfterScenario]
    public async Task Teardown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
        (_testService as IDisposable)?.Dispose();
    }

    [Given(@"I am on ""(.*)"" form page")]
    public async Task GivenIAmOnFormPage(string company)
    {
        await _page.GotoAsync($"http://localhost:5000/kontaktaoss/{company}");
        
    }

    [When(@"I select ""(.*)"" from the issue dropdown")]
    public async Task WhenISelectFromTheIssueDropdown(string issue)
    {
        var option = await _page.QuerySelectorAsync($"select#options >> text={issue}");
        if (option == null)
        {
            throw new Exception($"No valid option with text'{issue}'");
        }
        var value = await option.GetAttributeAsync("value");
        await _page.SelectOptionAsync("select#options", value);
    }
    
    [When(@"I input ""(.*)"" in the email textbox")]
    public async Task WhenIInputInTheEmailTextbox(string email)
    {
        await _page.FillAsync("input[id='email']", email);
    }

    [When(@"I input ""(.*)"" in the description textbox")]
    public async Task WhenIInputInTheDescriptionTextbox(string text)
    {
        await _page.FillAsync("textarea[id='description']", text);
    }

    [When(@"I submit the form")]
    public async Task WhenISubmitTheForm()
    {
        await _page.ClickAsync("button[type='submit']");
    }

    [Then(@"I should see the confirmation page")]
    public async Task ThenIShouldSeeTheConfirmationPage()
    {
        await _page.WaitForURLAsync(url => url.Contains("confirmation"));
    }

    [Given(@"I am on the loggin page")]
    public async Task GivenIAmOnTheLogginPage()
    {
        await _page.GotoAsync("http://localhost:5000/login");
    }

    [When(@"I input ""(.*)"" in the password textbox")]
    public async Task WhenIInputInThePasswordTextbox(string password)
    {
        await _page.FillAsync("input[id='password']", password);
    }

    [Then(@"I should see the relevant page based on my ""(.*)""")]
    public async Task ThenIShouldSeeTheRelevantPageBasedOnMyRole(string role)
    {
        if (role == "admin")
        {
            await _page.WaitForURLAsync(url => url.Contains("admin"));
        }
        else
        {
            await _page.WaitForURLAsync(url => url.Contains("arbetarsida"));
        }
    }


    [Given(@"I have submitted a form, with relevant info, ""(.*)"", ""(.*)"", ""(.*)"" and ""(.*)""")]
    public async Task GivenIHaveSubmittedAFormWithRelevantInfo(string company, string email, string issue, string text)
    {
        await _testService.MakeTicketForTest(email, issue, text);
        chatId = await _testService.GetChatIdForCustomerLogin(company, text, email);
        
        if (chatId == "")
        {
            Assert.Fail();
        }

        string formatedEmail = email.Replace("@", "%40");
        validChatLinkForLogin = chatId + "?email=" + formatedEmail;
    }


    [When(@"I click on the loggin link")]
    public async Task WhenIClickOnTheLogginLink()
    {
        await _page.GotoAsync($"http://localhost:5000/guestlogin/{validChatLinkForLogin}");
    }


    [Then(@"I should see my chat with my submitted ""(.*)""")]
    public async Task ThenIShouldSeeMyChatWithMySubmitted(string text)
    {
        await _page.WaitForURLAsync($"http://localhost:5000/chat/{chatId}");
        Assert.NotNull(_page.Locator($"text={text}"));
    }


    [When(@"I input ""(.*)"" in the textbox")]
    public async Task WhenIInputInTheTextbox(string text)
    {
        await _page.FillAsync("input[id='message']", text);
    }

    [Given(@"I am on my chatpage")]
    public async Task GivenIAmOnMyChatpage()
    {
        await _page.GotoAsync($"http://localhost:5000/guestlogin/{validChatLinkForLogin}");
        await _page.WaitForURLAsync($"http://localhost:5000/chat/{chatId}");
    }


    [Given(@"I am logged in with my credentials, ""(.*)"", ""(.*)""")]
    public async Task GivenIAmLoggedInWithMyCredentials(string email, string password)
    {
        await _page.GotoAsync("http://localhost:5000/login");
        await _page.FillAsync("input[id='email']", email);
        await _page.FillAsync("input[id='password']", password);
        await _page.WaitForURLAsync(url => url.Contains("arbetarsida") || url.Contains("admin"));
    }

    [Given(@"clicked the button to get to their chat")]
    public async Task GivenClickedTheLinkToGetToTheirChat()
    {
        await _page.ClickAsync($"a[href='/Chat/{chatId}']");
        await _page.WaitForURLAsync(url => url.Contains($"/Chat/{chatId}"));
    }
    
    [When(@"I submit the form with the send button")]
    public async Task WhenISubmitTheFormWithTheSendButton()
    {
        await _page.ClickAsync("button[textcontent='Send']");
    }

    [When(@"I submit the form with the Send and take next open ticket button")]
    public async Task WhenISubmitTheFormWithTheSendAndTakeNextOpenTicketButton()
    {
        await _page.ClickAsync("button[textcontent='Send and take next open ticket']");
    }


    [Then(@"I should be redirected to a new chat or to the arbetarsida")]
    public async Task ThenIShouldBeRedirectedToANewChatOrToTheArbetarsida()
    {
        await _page.WaitForURLAsync(url => !url.Contains($"/Chat/{chatId}"));
        // edgecase here that it'll fail in case of url changing from example /Chat/2 to /Chat/21 
    }

    [When(@"I click on the chat I want to assign myself and I'm sent to the chatpage")]
    public async Task WhenIClickOnTheChatIWantToAssignMyself()
    {
        var ParrentElement = _page.Locator($"a[href='/Chat/{chatId}']").Locator("..");
        var AssignTicketButton = ParrentElement.Locator("button[textcontent='Go to chat and assign ticket']");
        AssignTicketButton.ClickAsync();
        await _page.WaitForURLAsync(url => !url.Contains($"/Chat/{chatId}"));
    }

    [When(@"I go back to the arbetarsida")]
    public async Task WhenIGoBackToTheArbetarsida()
    {
        await _page.GotoAsync("http://localhost:5000/arbetarsida");
    }

    [Then(@"I should not see my assigned chat in the list")]
    public async Task ThenIShouldNotSeeMyAssignedChatInTheList()
    {
        var assignedChat = _page.Locator($"a[href='/Chat/{chatId}']");
        int timesChatAppears = await assignedChat.CountAsync();
        
        Assert.Equal(0, timesChatAppears);
    }

    [Then(@"I should see my assigned chat with my email, ""(.*)"", if i check the show all chats checkbox")]
    public async Task ThenIShouldSeeMyAssignedChatWithMyEmailIfICheckTheShowAllChatsCheckbox(string email)
    {
        await _page.CheckAsync("input[type='checkbox']"); //  works for this but I don't really like it
        
        var ParrentElement = _page.Locator($"a[href='/Chat/{chatId}']").Locator("../..");
        int timesChatAppears = await ParrentElement.CountAsync();
        Assert.NotEqual(1,timesChatAppears);
        
        int amIassigned = await ParrentElement.Locator($"small:has-text(\"{email}\")").CountAsync();
        Assert.Equal(1, amIassigned);
       
    }
}
