# Revive Adobe Flash on an up-to-date Windows 10 after 12 January 2021

Adobe Flash is finally dead. That's it, officially, but there are ways around.

## TL;DR

As an enterprise customer change to the Harman Packaged Browser. If not, essentially copy back the genuine Adobe Flash files from an older version of Windows, re-register the ActiveX component and use the custom application from this repository to load any Flash content.

## History

Back in mid 2017 Adobe [announced](https://blog.adobe.com/en/publish/2017/07/25/adobe-flash-update.html#gs.p38yhb) that they will be deprecating and dropping support for their Flash products by the end of 2020 and focusing on modern web technologies such as HTML, CSS and JavaScript.

Recently they've [announced](https://www.adobe.com/products/flashplayer/end-of-life.html) the actual End of Life of Flash player/plug-in as previously said and that it just won't be working from 12 January 2021. And that's true, having reached that date, any Flash content will only show a hint icon but not the actual content. This "kill switch" has silently been part of any Adobe Flash player/plug-in from version 24. However, version 23 is all right for further use. If you use it only for previously trusted Flash applications there's possibly no need to worry about security issues. There's also [more advice](https://www.adobe.com/products/flashplayer/enterprise-end-of-life.html) for enterprise customers who still rely on Flash today.

Browser manufacturers have been gradually dropping support for Flash plug-ins for a long period. First, one-time confirmations per site were required, then each time the site was loaded, then the Flash plug-ins had been disabled by default, and finally now the functionatlity is completely gone as soon as you install the most recent browser versions. This is backed by the Windows update [KB4577586](https://support.microsoft.com/de-de/help/4577586/update-for-removal-of-adobe-flash-player) which removes Flash support from the legacy Internet Explorer 11 and also removes the Flash ActiveX component from disk.

If you're an enterprise customer still using a Flash application even today, the only professional solution is offered by [Harman](https://services.harman.com/partners/adobe). That's the genuine Adobe Flash, exclusively licensed to Harman with three more years of support.

For nostalgia and less critical purposes have a look at the solution presented here: It's essentially coping back the genuine Adobe Flash files from an older version of Windows, re-register the ActiveX component, and use the custom application from this repository to load any Flash content.

## Steps to Revival

* To avoid any further disturbance (at least until Microsoft brings out additional related updates), install the Windows 10 update [KB4577586](https://support.microsoft.com/de-de/help/4577586/update-for-removal-of-adobe-flash-player) first.
* Get an ISO image of Windows 10 version 1607, because it still features Adobe Flash version 23. If you haven't got one at hand, you may use the "MediaCreationTool.bat" from [Github User AveYo](https://gist.github.com/AveYo/c74dc774a8fb81a332b5d65613187b15#file-mediacreationtool-bat) to download the correct image file. The most recent version from the Microsoft website will only download the most recent version of Windows.
* Get and install [7-Zip](https://www.7-zip.org/) or any other capable tool to extract the following directories from the archive `\sources\install.esd` from inside the image to your local Windows 10 installation:
    * `\1\Windows\System32\Macromed\Flash` to `C:\Windows\System32\Macromed\Flash` and
    * `\1\Windows\SysWOW64\Macromed\Flash` to `C:\Windows\SysWOW64\Macromed\Flash`.
* Import the Windows Registry file `Revive Flash ActiveX v23.reg` from this repository to re-register the Flash ActiveX component again. This might bring up an error message, that actually wasn't a problem in my case. If you're trying other versions of Adobe Flash, replace the version number (multiple occurrences) inside the Windows Registry file before importing.
* Make sure .NET Framework 3.5 is installed or do so via "Turn Windows features on or off".
* Download and extract the application "Custom Flash Viewer" from this repository's Releases page or clone the sources and compile them yourself with Visual C# 2008 Express or later.
* Edit the UTF-8 configuration file `cfv.cfg` and insert the absolute local path or a web URL to the Flash/SWF file to load as setting `Url=`. You cannot load any website, it's not a web browser. See [all configuration options.](#Custom-Flash-Viewer-Options)

### Custom Flash Viewer Options

|Option|Description|
|---|---|
|`Url=`|The absolute local file path or web URL to the SWF file to load.|
|`WindowTitle=`|The window title to display.|
|`DontUpdateConfig`|Don't write window size and position back to the configuration file when exiting the application. This option doesn't have a value.|

## Compatibility & Legal Questions
Running the recovered Adobe Flash ActiveX and the application from this repository will technically work with Windows XP and newer, including Windows Server 2003+, and Windows 7. However, as you're copying files from a Windows 10 ISO, I believe these files are solely licensed to you for usage with Windows 10 or maybe even solely for usage with that specific version of Windows 10. I also believe that downloading the Windows 10 ISO file also already requires a valid Windows 10 license, though it's technically possible without. I'll leave it to you as an exercise, to figure this out.

## Disclaimer

I'm not affiliated with any of Microsoft, Adobe, or Harman.

I'm not responsible for any damage caused directly or indirectly by applying the presented approach, neither technically, nor legally.

Before applying the presented approach, check with your legal adviser.

## License

All files in this repository are covered by the <a href="LICENSE">MIT License,</a> except for the Flash movie "sample.swf" which is licensed by Creative Commons under [CC BY-NC-SA.](https://creativecommons.org/licenses/by-nc-sa/1.0/)