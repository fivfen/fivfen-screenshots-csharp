using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using FivFen;

public class UrlTests
{
    private FivFen fivfen;

    public void TestInitialize()
    {
        fivfen = new FivFen("MY_API_KEY", "secret");
    }

    //public void WithOptions()
    //{
    //    dynamic options = new ExpandoObject();
    //    options.url = "bbc.com";
    //    options.Width = 1280;
    //    options.Thumb_Width = 500;
    //    options.Full_Page = true;

    //    var output = fivfen.GenerateUrl(options);
    //    Assert.AreEqual("https://fivfen.com/api?v=v1&key=MY_API_KEY&url=bbc.com&width=1280&thumb_width=500&full_page=true",
    //                    output, "Not OK");
    //}

    public void WithUrlEncodedOptions()
    {
        dynamic options = new ExpandoObject();
        options.url = "bbc.com";
        options.Width = 1280;
        options.Thumb_Width = 500;
        options.FullPage = true;
        options.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

        var output = fivfen.GenerateUrl(options);
        Assert.AreEqual("https://fivfen.com/api?v=v1&key=MY_API_KEY&sec=9c675714240421b50a9f76892d702cb0a5376ccf&url=bbc.com&width=1280&thumb_width=500&full_page=true&user_agent=Mozilla%2F5.0%20%28Windows%20NT%206.1%29%20AppleWebKit%2F537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome%2F41.0.2228.0%20Safari%2F537.36",
                        output, "Not OK");
    }

    public void UrlNeedsEncoding()
    {
        dynamic options = new ExpandoObject();
        options.url = "https://www.hatchtank.io/markup/index.html?url2png=true&board=demo_1645_1430";
        var output = fivfen.GenerateUrl(options);
        Assert.AreEqual("https://fivfen.com/api?v=v1&key=MY_API_KEY&sec=9c675714240421b50a9f76892d702cb0a5376ccf&url=https%3A%2F%2Fwww.hatchtank.io%2Fmarkup%2Findex.html%3Furl2png%3Dtrue%26board%3Ddemo_1645_1430",
                        output, "Not OK");
    }

    public void WithUserAgent()
    {
        dynamic options = new ExpandoObject();
        options.Url = "https://bbc.com";
        options.User_Agent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";

        var output = fivfen.GenerateUrl(options);
        Assert.AreEqual("https://fivfen.com/api?v=v1&key=MY_API_KEY&sec=9c675714240421b50a9f76892d702cb0a5376ccf&url=https%3A%2F%2Fbbc.co.uk&user_agent=Mozilla%2F5.0%20%28Macintosh%3B%20Intel%20Mac%20OS%20X%2010_12_6%29%20AppleWebKit%2F537.36%20%28KHTML%2C%20like%20Gecko%29%20Chrome%2F62.0.3202.94%20Safari%2F537.36", output);
    }

    public void IgnoreEmptyValuesAndFormat()
    {
        dynamic options = new ExpandoObject();
        options.Url = "https://bbc.com";
        options.Full_Page = false;
        options.ThumbWidth = "";
        options.Delay = null;
        options.Format = "pdf";
        options.Selector = "";
        options.WaitFor = "";

        var output = fivfen.GenerateUrl(options);
        Assert.AreEqual("https://fivfen.com/api?v=v1&key=MY_API_KEY&sec=9c675714240421b50a9f76892d702cb0a5376ccf&url=https%3A%2F%2Fbbc.com&full_page=false",
                        output, "Not OK");
    }

    public void FormatWorks()
    {
        dynamic options = new ExpandoObject();
        options.url = "bbc.com";
        var output = fivfen.GenerateUrl(options);
        Assert.AreEqual("https://fivfen.com/api?v=v1&key=MY_API_KEY&sec=9c675714240421b50a9f76892d702cb0a5376ccf&url=bbc.com", output, "Not OK!");
    }

    public void WithoutUrl()
    {
        dynamic options = new ExpandoObject();
        //options.Width = 500;
        options.full_page = true;
        var output = fivfen.GenerateUrl(options);
        Assert.IsTrue(true);
    }

    public void SimpleURL()
    {
        dynamic options = new ExpandoObject();
        options.url = "bbc.com";
        var output = fivfen.GenerateUrl(options);
        Assert.AreEqual("https://fivfen.com/api?v=v1&key=MY_API_KEY&sec=9c675714240421b50a9f76892d702cb0a5376ccf&url=bbc.com",
                        output, "Not OK");
    }
}

public class DownloadTests
{
    private FivFen fivfen;

    public void TestInitialize()
    {
        fivfen = new FivFen("MY_API_KEY", "secret");
    }

    public async Task TestDownloadToFile()
    {
        var fivfenUrl = "https://fivfen.com/api?v=v1&key=MY_API_KEY&url=google.com";
        var result = await fivfen.DownloadToFile(fivfenUrl, "result.png");
        //Debug.WriteLine(result, "RESULT - Download");
        Assert.IsTrue(true);
    }

    public async Task TestDownloadBase64()
    {
        var fivfenUrl = "https://fivfen.com/api?v=v1&key=MY_API_KEY&url=bbc.com";
        var base64result = await fivfen.DownloadAsBase64(fivfenUrl);
        //Debug.WriteLine(base64result, "RESULT - BASE64");
        Assert.IsTrue(true);
    }

    public async Task TestDownloadFail()
    {
        var fivfenUrl = "https://fivfen.com/api?v=v1&key=MY_API_KEY&url=bbc.com";
        var base64result = await fivfen.DownloadAsBase64(fivfenUrl);
        Debug.WriteLine(base64result, "RESULT - BASE64");
        Assert.IsTrue(true);
    }
}
