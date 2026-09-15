---
title: "Git Basics - Exercise"
source: "csfiles/home_dir/GitIntro/Git Basics - Exercise.pdf"
modul: "Lektion 01.2: Exceptions + Git Workflow"
pages: 4
type: "dokument"
vision: "n/a"
---
# Git Basics - Exercise

<!-- side 1 -->

                                                                                                      Git Basics - Exercise.docx
                                                                                                                   31-01-2025


1     Lab Exercise: Git basics with the Calculator

In this exercise you will use and extend your previous Calculator-exercise to practice using Git. Thus, the focus here is
not on the software, but on using Git properly!


Remember! Whenever you have done a change (implemented a new test or a working part of a method), commit
your work with a decent commit comment. When you have finished a suitable amount of work, do a Pull – Test – Push
cycle, as explained in the lecture slides.




1.1     Install Git
All students: Install Git Tools – as described on Brightspace for Windows and Visual Studio. For other platforms, you
need a Git installation with at least command line interface. An integration with your code development environment
(IDE) may also be helpful.

1.2   Make groups
Team up in teams of 3-4 persons via Brightspace – use “Gruppetilmelding”. This should be your groups for handing in
the mandatory exercises.

1.3     Register team members on gitlab.au.dk
All students in the group/team must login on https://gitlab.au.dk which you will use to store remote repositories.
Use the “Sign in with” “UNI-AD” button at the bottom of the page, and login with your AU account. This will demand
Two Factor Authorization (TFA), so have your TFA-solution ready (normally an app on your smartphone). This first
login is necessary to register you as a known user on gitlab.au.dk.
After you have been accepted as a user, you can go to your user profile. To access your repositories from several tools
it can be beneficial to set up an SSH key-pair. With SSH you generate a private and a public key. The private key is
stored on your local computer and must be kept private. The public key is then copied to the server, that you wishes
to authenticate with. As you later try to connect to the server with SSH, your local computer and the server will
exchange keys and authenticate that you are actually you. To set this up do the following:
     1. Logged into gitlab.au.dk, open User Settings->SSH Keys. Press “Add new key”. A new ssh public key can be
          pasted into the text box. Press “learn more” to …learn more about ssh keys, it also describes how to generate
          ssh-keys on your local computer
     2. On your PC open a terminal (Windows: PowerShell) and type “ssh-keygen”. This will generate a new keypair
          stored in two files. Ssh-keygen will tell you where it has stored the two files with the keys, typically .ssh/ in
          your home folder (Windows: c:\Users\Peter\.ssh) (Linux/Mac: /home/Peter/.ssh)
     3. Locate the .pub file in the .ssh folder and open it with a text editor. Copy the text to your clipboard
     4. Go back to the browser and paste the public key into the text box mentioned in 1. You can set an expiration
          date if you prefer. Finally, press “Add Key”. That’s it, now your local PC is authenticated to be used with your
          gitlab.au.dk account. Repeat if you have more computers!

1.4    Create the repository with access for the team
One team member should now create a Group in Gitlab and add fellow members to the group using their respective
AU ID. Inside the newly created group you can now create an empty repository. (Visual Studio cannot do that
automatically on gitlab.au.dk as it can on github.com).
Use the ‘+’ menu to the left of your avatar on the top of page, select “New Project/Repository”.
Use the “Create a Blank Project” template.
Make it internal, so that logged in students and teachers can see your project for the mandatory hand-ins.
At this point do not set the check mark for a README file, because we wish to push an existing project to the Gitlab
server.


                                                         Page 1 of 4

<!-- side 2 -->

                                                                                                        Git Basics - Exercise.docx
                                                                                                                     31-01-2025
The group you have create can be used to host repositories for exercises and for the hand-ins as well.

1.5     Connect your solution and the new repository
First of all, chose among your team members the one with the most complete solution to the Calculator exercise. This
person will now make the connection. The rest of the team will from now work on this, instead of their own personal
solution.
Make sure the solution can be built and that the tests can run and are all passing.
There are several ways to make the connection.
    1. Use the built-in support in Visual Studio to work with a remote repository
    2. Use the command line interface to git, working from the solution folder
    3. Use whatever support your IDE (perhaps with extensions) has that can work with a remote repository
    4. Github Desktop (Taking care to use non-Github repositories!)
    5. (and many more)
The first 2 are described below.

1.5.1     Connect with Visual Studio 2022
To simultaneously create a local repository and push to a remote repository, select “Add to Source Control” on the VS
status line, or the “Git/Create Git Repository” menu command, or the “Create Git Repository” button in the “Git
Changes” view. The following dialogue box will appear.




Select “Existing remote” and insert the path to your GitLab project as the Remote URL. As you can see, the user is
probably shown as the person’s AU id. The URL must of course be the GitLab repository you created in section 1.4.
You can choose to create both a local repository and pushing to GitLab at the same time. Don’t worry if you only make
a local repository, it can be “Pushed” to GitLab later on.
After the local repository has been created, on Windows, you may see a weird 90s style pop-up a dialogue box saying
something like “SSH: The authenticity of host <host> (gitlab.au.dk) can't be established”. Those who have tried
connecting to a new server using ssh before, will know that it wants you accept the connection, therefore you will
have to type “yes” and press enter!


Make sure a “.gitignore” file has been created. The file name is a strange one, as it starts with a period (‘.’). The
.gitignore file acts as a filter to limit which file types to manage under version control. Either use the “Git/Settings/Git
Repository Settings” command to check that it is set or use the File Explorer to check it is in the solution folder (Show
Hidden Items must be checked in the File Explorer). If you do not have a .gitignore file, you can create one by opening
a terminal/PowerShell in your project root folder and type: “dotnet new gitignore”.
Open the Git Changes view. From here Add/Commit/Push/Pull/and Sync can be executed. Sync is the same as Pull +
Push. Only source and project files (and documents) should be added, if binary files appear in the “Git Changes” view,
then probably your .gitignore fil is not setup correctly.




                                                         Page 2 of 4

<!-- side 3 -->

                                                                                                        Git Basics - Exercise.docx
                                                                                                                     31-01-2025
1.5.2     Connect from the command line
You must start up a command line box – either GitBash, Windows cmd, PowerShell, terminal or whichever command
line interface your platform gives you.
Make sure you are in the correct directory/folder. Normally the current directory will be shown as part of the
command prompt. You must be in the same directory as the .sln file. Use cd to get there. Use ls or dir to list the
files and folders.
To create the local repository, you must type:
git init --initial-branch=main
Before you add your files to the local repository, you must be sure, you have a “.gitignore” file suitable for DotNet
development and your IDE. You can create one with:
dotnet new gitignore
Now you can add the (filtered) files to the staging index:
git add .
And create a new version:
git commit -m "Initial commit"
Setup the connection (here: origin) to the remote repository, (here: au237297/gitlabexp5) :
git remote add origin https://gitlab.au.dk/au237297/gitlabexp5.git
Push the new version to the branch “main” on the remote server (origin):
git push -u origin main
The URL must of course be the GitLab repository you created in section 1.4.
You can only push to an empty repository! If you checked “Create README.md” in Gitlab, it’s not empty. In this case,
you will have to clone the repo into another folder (not a subfolder of the current project!!) and copy the project files
(not the stuff in .git!!) into the folder that you have just cloned into. Then add a .gitignore in the new folder, do git add
. commit and push.


1.6    Clone the remote repository
Now it’s time for the other team members to get a working copy of the common repository. Use the SSH Url found in
the Gitlab repository when pressing the blue “Code” button.
There are several ways to do this
              1.    When starting VS, there is an option to clone a project. It has several ways to do this
              2.    There are other ways to clone from VS, the Git menu Clone Repository command is one of them
              3.    Use the command line to call the git clone <url> command. This must be done in the folder,
                    where the cloned folder should be created – so it will become the parent folder of a new folder with
                    the clone.
After cloning, all other team members shall try to build the solution on their own PC.


1.7   Coding exercise
Now for some coding to practice cooperating through a common remote repository.
Add the following functionality to the Calculator or other relevant features you think could be interesting – let your
imagination run.
Make decisions on details of operations and exceptions as necessary.
Make the relevant unit tests for the class for operations and exceptions.
Be sure to divide the work between you, so you work in parallel!




                                                          Page 3 of 4

<!-- side 4 -->

                                                                                                   Git Basics - Exercise.docx
                                                                                                                31-01-2025

public double Divide(                                   Return the result of the division dividend/divisor.
    double dividend, double divisor)                    Consider what should happen when a division by zero is
                                                        attempted.
                                                        Test it.
public double Accumulator {get; private set; }          Add the C# property Accumulator to the Calculator
                                                        class. It should always contain the result of the latest
                                                        operation.
                                                        Consider what it should contain, initially and when
                                                        errors happen. Test it accordingly.
public void Clear()                                     This method should clear the accumulator to contain
                                                        zero.
public double Add(double addend)                        The feature Accumulator is a prerequisite for these
public double Subtract(double subtractor)               features.
public double Multiply(double multiplier)               Make overloads of the calculation functions with one
                                                        parameter less than the original. The missing operand is
public double Divide(double divisor)
                                                        always taken from the accumulator, thus making it
public double Power(double exponent)                    possible to make chained calculations like on a real
                                                        calculator.
                                                        The methods returns the result and changes the
                                                        accumulator.
                                                        Consider error situations.
                                                        Test everything.
(no new function)                                       Check the result of your Power function. Are you
                                                        satisfied with the results from strange combinations of
                                                        operands, e.g. negative x with non-integer exponent?
                                                        As a hint, check the specification for the C# Pow
                                                        function (if that's the one, you are using)
                                                        Do you need another exception?
                                                        Test the implementation of your decisions.


Remember!
    •   Whenever you have done a change (implemented a new test or a working (part of a) method), commit your
        work with a decent commit comment.
    •   Push your changes to the distributed Git repository when you have something to share. Remember, before
        you push, pull the repository and solve any merge conflicts (due to previous commits from other team
        members), and test, if any changes were pulled or had to be merged. Merge is most easily done using Visual
        Studio’s built-in three-way merge.




                                                     Page 4 of 4

