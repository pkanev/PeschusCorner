namespace Peschu.Services.Helpers
{
    // Clipboard Fusion Macro: "Convert YouTube/Vimeo URL to Embed Code"
    // Michael Seman (@AphixJS)
    // v1.0.0
    // 2017-02-22
    // Modified

    using System;

    public static class ClipboardFusionHelper
    {
        public static string ProcessText(string url)
        {
            if (url.Contains("youtube.com") || url.Contains("youtu.be"))
            {
                return ClipboardFusionHelper.ConvertYouTubeToEmbed(url);
            }

            if (url.Contains("vimeo.com"))
            {
                return ClipboardFusionHelper.ConvertVimeoToEmbed(url);
            }

            return url;
        }

        private static string ConvertYouTubeToEmbed(string url)
        {
            var watchKeyWord = "watch?v=";
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string idWithPotentialQueryParams = parts[parts.Length - 1].ToString().Replace("?t=", "?start=");
            if(idWithPotentialQueryParams.StartsWith(watchKeyWord))
            {
                idWithPotentialQueryParams = idWithPotentialQueryParams.Replace(watchKeyWord, string.Empty);
            }
            //string joiner = idWithPotentialQueryParams.Contains("?") ? "&" : "?";
            //string suffix = "ecver=2";
            var joinerIndex = idWithPotentialQueryParams.IndexOf('&');
            if(joinerIndex != -1)
            {
                idWithPotentialQueryParams = idWithPotentialQueryParams.Substring(0, joinerIndex);
            }
            return "https://www.youtube.com/embed/" + idWithPotentialQueryParams;// + joiner + suffix;
        }

        private static string ConvertVimeoToEmbed(string url)
        {
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string id = parts[parts.Length - 1].ToString();
            return "https://player.vimeo.com/video/" + id;
        }
    }
}
