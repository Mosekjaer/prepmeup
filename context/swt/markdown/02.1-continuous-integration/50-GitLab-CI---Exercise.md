---
title: "GitLab CI - Exercise"
source: "csfiles/home_dir/ContinuousIntegration/GitLab CI - Exercise.pdf"
modul: "Lektion 02.1: Continuous Integration"
pages: 3
type: "dokument"
vision: "n/a"
---
# GitLab CI - Exercise

<!-- side 1 -->

                                                                                                      GitLab CI - Exercise.docx
                                                                                                                    05-09-2023




Lab Exercise: Setting up your GitLab project for Continuous Integration

In this exercise you will setup Continuous Integration on the GitLab server, with build and test of the Calculator-
exercise your started last week.

Your GitLab repository will be set up with a Continuous Integration file, which will trigger build and test every time you
push something to the repository. You can then check the result of the build and tests in this controlled environment.
You will receive an e-mail when something fails.

Remember! Whenever you have done a change (implemented a new test or a working part of a method), commit
your work with a decent commit comment. When you have finished a suitable amount of work, do a Pull – Test – Push
cycle, as explained in the lecture slides.


    1.   Prepare your test project for using dotnet on GitLab as the test runner instead of Resharper: Use the NuGet
         manager to install:
               • JunitXml.TestLogger.

         On top of that, also be sure that the:
                 • NUnit
                 • Microsoft.Net.Test.SDK
                 • and the NUnit3TestAdapter

         packages are also in your test project. If you used the NUnit test project template, the last 3 should already
         be there.

    2.   A YAML file describing the necessary steps to Build and Test your Calculator must now be added to your solu-
         tion files. The name of the files must be ".gitlab-ci.yml", it must be placed in the solution folder – the same
         folder as your .sln file and your .git folder is located, and its content must match the file with this name that
         can be found on Brightspace under the material this weeks lectures.

         There are several ways to do this, among these:
             1. Download the file from Brightspace and place it in the correct folder. I you want to see or edit it
                 from Visual Studio, use the Add Existing Item on your solution.
             or
             2. Create a file using the Add New Item command on your solution with the correct name, and
                 copy/paste the content from BrightSpace
             or
             3. Create the file on GitLab using the GitLab file editors to add the content.

    3.   In the two first cases, commit the new file and then do a Push to GitLab. In the last case, once you have
         committed it, you should make a Pull from GitLab first.

    4.   After step 3, a Pipeline should start executing on GitLab. You can see it under the Build (was previously CI/CD)
         left side menu command, selecting Pipelines:




                                                        Page 1 of 3

<!-- side 2 -->

                                                                                                  GitLab CI - Exercise.docx
                                                                                                                05-09-2023




5.   When its done, click on the "Passed" label for the Pipeline run and this picture should appear:




6.   When you click on Tests you should be able to expand all your test fixtures and see all the tests that passed
     or that failed.

7.   A Pipeline can be marked as failed for several reasons:

         •    There can be an error in the Pipeline script ".gitlab-ci.yml"
         •    There can be a compile/build error
         •    There can be an error in running the tests
         •    There can be one or more tests that run but are failing

     To investigate, click on Failed Jobs, this should tell you what went wrong. If it wasn't because of a failed test
     (which should mean there is an error in you UUT (Unit Under Test), you can click on the "build-and-test" but-
     ton, which will open the console output from running the job. From this you should be able to deduce what
     the problem is.




                                                    Page 2 of 3

<!-- side 3 -->

                                                                                              GitLab CI - Exercise.docx
                                                                                                            05-09-2023




8.  Now, another one from your team shall create a change in the project. Build and Test locally. Commit the
    change to the local repository. Finally do a sync: Pull – Push to your remote GitLab repository.
9. Goto the GitLab server. After a few seconds (10-15), a new Pipeline for your project should start automatical-
    ly.
10. Continue working in parallel on the Calculator, syncing when you have a working change with tests. Use the
    suggestions for work that are in the Lab Exercise from Lecture 01.2.




                                                  Page 3 of 3

