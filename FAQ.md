# FAQ

### Is it safe to give my AttackPoint credentials to the plugin?
Your password is stored encrypted on your local hard drive. Your credentials are transmitted only between the plugin and AttackPoint website the same way your browser does it.

### The plugin keeps warning me about unmapped shoes and/or unspecified intensity. How can I disable the warnings?
Go to plugin settings page and uncheck the corresponding checkbox.

### How do I format Notes?
Check "Formatting Rules" in [Getting Started](GettingStarted.pdf) for details.

### I entered some text into the Notes field but it was not exported.
Check that you have `{Notes}` placeholder specified in the Notes Format field in plugin settings. Leave the Notes Format field blank if you want your notes to be exported exactly as you enter them. See "Formatting Rules" in [Getting Started](GettingStarted.pdf) for details.

### Can I edit my training/note after I exported it?
Yes. Go to your AttackPoint training log, choose the training, and click "Edit" link. Modify your training or click "Delete" link to remove the training from your log.

### Where are the plugin settings stored?
Click Start->Run and type `%UserProfile%\Local Settings\Application Data\ZoneFiveSoftware\SportTracks\2.0` to open the folder. The settings are stored in `Preferences.Preferences.xml` file. This is SportTracks main configuration file. Open it in a text editor and search for "`AttackPointConfiguration`" section. It is a good idea to back up this file.

### I don't see HR Zones Mapping tab. The mixed intensity text boxes are disabled. I can't enter Private Note. I can't export GPS track.
You don't have AttackPoint bonus features enabled because you haven't donated to AttackPoint.

### I have added new activity and/or shoes in AttackPoint profile. I don't see them in plugin settings.
Click "Retrieve" button to synchronize your AttackPoint profile with plugin settings. Configure the mapping afterwards.

### Where is the plugin's log file located?
It is located in `%UserProfile%\Local Settings\Application Data\ZoneFiveSoftware\SportTracks`. The file is called `attackpoint-plugin.log`.

### Where are the plugin's binaries located?
They are located in `%ALLUSERSPROFILE%\Application Data\ZoneFiveSoftware\SportTracks\2.0\Plugins\Installed\1eec167a-0605-479e-8def-08633ac68a22` folder.

### Sorry it didn't work for me. I want to uninstall this thing.
Close SportTracks. Click Start->Run and type in the following:
"`%ALLUSERSPROFILE%\Application Data\ZoneFiveSoftware\SportTracks\2.0\Plugins\Installed`". Windows Explorer will open and you should see a list of folders one of which is called `1eec167a-0605-479e-8def-08633ac68a22`. Remove this folder.