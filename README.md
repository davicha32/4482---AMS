





<!DOCTYPE html>
<html lang="en">
  <div id="readme" class="Box-body readme blob instapaper_body js-code-block-container">
    <article class="markdown-body entry-content p-3 p-md-6" itemprop="text"><h1><a id="user-content-asset-management-system" class="anchor" aria-hidden="true" href="#asset-management-system"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>Asset Management System</h1>
<hr>
<h2><a id="user-content-about-project" class="anchor" aria-hidden="true" href="#about-project"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>About Project</h2>
<p>IT offices all over the country have to contend with small budgets daily. One of the most important aspects of an IT office is the ability to track and resolve issues that their customers have. These are known as tickets, and the software to track them can be incredibly expensive. Many IT administrators must then turn to an open source approach. Open source ticket tracking systems often have their problems however. Namely, they are very good at tracking tickets, but have little to no integration to track assets.</p>
<p>That's where this program shines. AMS, or Asset Management System, combines the utility of programs such as Request Tracker and SnipeIT, so an IT office can track tickets and assets in the same system. They can also assign specific assets to tickets. This will create an invaluable history tracking the lifetime of a particular asset. With the usage of a relational database, tickets are tracked, notes and other key information is stored, assets are managed, and an IT office is able to handle their day to day operations through a single dynamic program.</p>
<h2><a id="user-content-first-usage-notes" class="anchor" aria-hidden="true" href="#first-usage-notes"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>First Usage Notes</h2>
<ol>
<li>
<p>Load the database</p>
</li>
<li>
<p>Login with the standard admin account:</p>
<ol>
<li>Email: <code>cobpcs@gmail.com</code></li>
<li>Password: <code>tester</code></li>
</ol>
</li>
<li>
<p>Create any Roles on top of the base set:</p>
<ol>
<li>Administrators have all rights</li>
<li>Technicians can add and edit in most cases</li>
<li>Base is for users who are not intended to be able to log in; creates a list for requestors</li>
</ol>
</li>
<li>
<p>Create Users</p>
<ol>
<li>Add an account for yourself as an admin</li>
<li>Add accounts for any technicians you may employ</li>
<li>Add any other users you will need</li>
</ol>
<ul>
<li><strong><em>NOTE: make sure your technicians and other roles that can log in change their passwords when they log in the first time</em></strong></li>
</ul>
</li>
<li>
<p>Create States for Assets and Locations (ex. Working, Broken, On Fire etc.)</p>
</li>
<li>
<p>Add Locations</p>
<ol>
<li>This will be needed when you create an asset – your assets will be assigned to a location</li>
</ol>
</li>
<li>
<p>Create your assets – brands, models and device types can be added there</p>
</li>
<li>
<p>Create tickets</p>
<ol>
<li>When a ticket is created an email is sent to the associated requestor's email</li>
</ol>
</li>
</ol>
<h2><a id="user-content-dependency-installation" class="anchor" aria-hidden="true" href="#dependency-installation"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>Dependency Installation</h2>
<h4><a id="user-content-aspnet-core" class="anchor" aria-hidden="true" href="#aspnet-core"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>ASP.NET Core</h4>
<p>Since this application has been developed on ASP.NET Core it's usage is cross platform. There are only a couple of dependencies that need to be installed.</p>
<p>First download the appropriate dotnet core installation for your system from here:</p>
<p><a href="https://dotnet.microsoft.com/download" rel="nofollow">https://dotnet.microsoft.com/download</a></p>
<p>When choosing which version to get, it is best to get the SDK installer since that will include all of the build packages needed for this project. The SDK also includes the Runtime packages.</p>
<p>At the time of this applications creation, the current release is v2.2.4 of which the SDK is 2.2.203</p>
<p><a href="https://dotnet.microsoft.com/download/dotnet-core/2.2" rel="nofollow">https://dotnet.microsoft.com/download/dotnet-core/2.2</a></p>
<h5><a id="user-content-privacy-notice" class="anchor" aria-hidden="true" href="#privacy-notice"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a><em>Privacy Notice</em></h5>
<p>ASP.NET Core much like other Microsoft products has a telemetry feature builtin and enabled by default. To disable this "feature" you will need to follow the steps mentioned on this page:</p>
<p><a href="https://docs.microsoft.com/en-us/dotnet/core/tools/telemetry" rel="nofollow">https://docs.microsoft.com/en-us/dotnet/core/tools/telemetry</a></p>
<p>In short you need to set the <code>DOTNET_CLI_TELEMETRY_OPTOUT</code> variable to either <code>1</code> or <code>true</code>.</p>
<h4><a id="user-content-mariadb--mysql" class="anchor" aria-hidden="true" href="#mariadb--mysql"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>MariaDB / MySQL</h4>
<p>The next package you will need is a database to store the values the application manages. We utilized a MariaDB / MySQL installation. TheDatabase.sql script is compatible with either system.</p>
<p>On Windows and Mac our developers used the XAMPP installation package to get a MariaDB system up and running:</p>
<p><a href="https://www.apachefriends.org/download.html" rel="nofollow">https://www.apachefriends.org/download.html</a></p>
<p>To configure the database with that the developers used <code>localhost/phpmyadmin</code> in their browsers. Running the SQL script, to clear out and reload the database whenever there was a change.</p>
<p>On Linux the developers used the package available in the repository of Fedora, and the included bash script to reload the database when needed.</p>
<h2><a id="user-content-server-usage" class="anchor" aria-hidden="true" href="#server-usage"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>Server Usage</h2>
<p>For server usage, the application is already configured to run on port 8080. This port number and configuration can be changed in the <code>Program.cs</code> file as well as the <code>launchSettings.json</code> in the Properties folder.</p>
<p>The team used a Fedora 29 KDE installation to host the server. Once the computer had all of the dependencies installed, and to help in the server setup, there are 4 bash script files in the application root directory.</p>
<ul>
<li>
<p><code>0-completeSetup.sh</code> Runs the 3 scripts in sequence</p>
</li>
<li>
<p><code>1-gitPull.sh</code> Pulls the latest version from the git repository that this application is stored at</p>
</li>
<li>
<p><code>2-updateDatabase.sh</code> Reloads the database based on the Database.sql file</p>
</li>
<li>
<p><code>3-runServer.sh</code> Compiles the application and then runs it. Upon application start the server will be ready to go for connections on port 8080</p>
</li>
</ul>
<h2><a id="user-content-editors-used" class="anchor" aria-hidden="true" href="#editors-used"><svg class="octicon octicon-link" viewBox="0 0 16 16" version="1.1" width="16" height="16" aria-hidden="true"><path fill-rule="evenodd" d="M4 9h1v1H4c-1.5 0-3-1.69-3-3.5S2.55 3 4 3h4c1.45 0 3 1.69 3 3.5 0 1.41-.91 2.72-2 3.25V8.59c.58-.45 1-1.27 1-2.09C10 5.22 8.98 4 8 4H4c-.98 0-2 1.22-2 2.5S3 9 4 9zm9-3h-1v1h1c1 0 2 1.22 2 2.5S13.98 12 13 12H9c-.98 0-2-1.22-2-2.5 0-.83.42-1.64 1-2.09V6.25c-1.09.53-2 1.84-2 3.25C6 11.31 7.55 13 9 13h4c1.45 0 3-1.69 3-3.5S14.5 6 13 6z"></path></svg></a>Editors Used</h2>
<p>This application was created with two different IDE's. First was Microsoft Visual Studio Enterprise 2015 / 2017 / 2019 (certain team members had different versions).</p>
<p>Microsoft Visual Studio (Windows):</p>
<p><a href="https://visualstudio.microsoft.com/" rel="nofollow">https://visualstudio.microsoft.com/</a></p>
<p>Further along in the development the IDE was switched over to JetBrains Rider, to better debug and refactor the code as needed.</p>
<p>JetBrains Rider (Linux / Mac / Windows):</p>
<p><a href="https://www.jetbrains.com/rider/" rel="nofollow">https://www.jetbrains.com/rider/</a></p>
<p>Each IDE was used with the educational edition to allow for all available features.</p>
<p>Created by: Tyson Baker, Jack Bradley, Chas Davis, Aaron Harvey, Julia Parsley</p>
</article>
  </div>

    </div>
  </body>
</html>

