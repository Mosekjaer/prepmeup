---
title: "Pro Git — Branching and Merging"
source: "progit.pdf"
modul: "Supplerende laesning (uddrag)"
pages: "62-77"
type: "bog-uddrag"
pensum: false
---

# Pro Git — Branching and Merging (bogens s. 62-77)

<!-- side 62 -->

  $ git last
  commit 66938dae3329c7aebe598c2246a8e6af90d04646
  Author: Josh Goebel <dreamer3@example.com>
  Date:   Tue Aug 26 19:48:51 2008 +0800

      Test for current head

      Signed-off-by: Scott Chacon <schacon@example.com>


As you can tell, Git simply replaces the new command with whatever you alias it for. However,
maybe you want to run an external command, rather than a Git subcommand. In that case, you
start the command with a ! character. This is useful if you write your own tools that work with a
Git repository. We can demonstrate by aliasing git visual to run gitk:


  $ git config --global alias.visual '!gitk'



Summary
At this point, you can do all the basic local Git operations — creating or cloning a repository, making
changes, staging and committing those changes, and viewing the history of all the changes the
repository has been through. Next, we’ll cover Git’s killer feature: its branching model.




62

<!-- side 63 -->

Git Branching
Nearly every VCS has some form of branching support. Branching means you diverge from the
main line of development and continue to do work without messing with that main line. In many
VCS tools, this is a somewhat expensive process, often requiring you to create a new copy of your
source code directory, which can take a long time for large projects.

Some people refer to Git’s branching model as its “killer feature,” and it certainly sets Git apart in
the VCS community. Why is it so special? The way Git branches is incredibly lightweight, making
branching operations nearly instantaneous, and switching back and forth between branches
generally just as fast. Unlike many other VCSs, Git encourages workflows that branch and merge
often, even multiple times in a day. Understanding and mastering this feature gives you a powerful
and unique tool and can entirely change the way that you develop.


Branches in a Nutshell
To really understand the way Git does branching, we need to take a step back and examine how Git
stores its data.

As you may remember from What is Git?, Git doesn’t store data as a series of changesets or
differences, but instead as a series of snapshots.

When you make a commit, Git stores a commit object that contains a pointer to the snapshot of the
content you staged. This object also contains the author’s name and email address, the message that
you typed, and pointers to the commit or commits that directly came before this commit (its parent
or parents): zero parents for the initial commit, one parent for a normal commit, and multiple
parents for a commit that results from a merge of two or more branches.

To visualize this, let’s assume that you have a directory containing three files, and you stage them
all and commit. Staging the files computes a checksum for each one (the SHA-1 hash we mentioned
in What is Git?), stores that version of the file in the Git repository (Git refers to them as blobs), and
adds that checksum to the staging area:


  $ git add README test.rb LICENSE
  $ git commit -m 'Initial commit'


When you create the commit by running git commit, Git checksums each subdirectory (in this case,
just the root project directory) and stores them as a tree object in the Git repository. Git then creates
a commit object that has the metadata and a pointer to the root project tree so it can re-create that
snapshot when needed.

Your Git repository now contains five objects: three blobs (each representing the contents of one of
the three files), one tree that lists the contents of the directory and specifies which file names are
stored as which blobs, and one commit with the pointer to that root tree and all the commit
metadata.




                                                                                                       63

<!-- side 64 -->

Figure 9. A commit and its tree

If you make some changes and commit again, the next commit stores a pointer to the commit that
came immediately before it.




Figure 10. Commits and their parents

A branch in Git is simply a lightweight movable pointer to one of these commits. The default branch
name in Git is master. As you start making commits, you’re given a master branch that points to the
last commit you made. Every time you commit, the master branch pointer moves forward
automatically.


                 The “master” branch in Git is not a special branch. It is exactly like any other
                branch. The only reason nearly every repository has one is that the git init
                 command creates it by default and most people don’t bother to change it.




64

<!-- side 65 -->

Figure 11. A branch and its commit history


Creating a New Branch

What happens when you create a new branch? Well, doing so creates a new pointer for you to
move around. Let’s say you want to create a new branch called testing. You do this with the git
branch command:


  $ git branch testing


This creates a new pointer to the same commit you’re currently on.




Figure 12. Two branches pointing into the same series of commits

How does Git know what branch you’re currently on? It keeps a special pointer called HEAD. Note
that this is a lot different than the concept of HEAD in other VCSs you may be used to, such as
Subversion or CVS. In Git, this is a pointer to the local branch you’re currently on. In this case,
you’re still on master. The git branch command only created a new branch — it didn’t switch to that


                                                                                                65

<!-- side 66 -->

branch.




Figure 13. HEAD pointing to a branch

You can easily see this by running a simple git log command that shows you where the branch
pointers are pointing. This option is called --decorate.


  $ git log --oneline --decorate
  f30ab (HEAD -> master, testing) Add feature #32 - ability to add new formats to the
  central interface
  34ac2 Fix bug #1328 - stack overflow under certain conditions
  98ca9 Initial commit


You can see the master and testing branches that are right there next to the f30ab commit.


Switching Branches

To switch to an existing branch, you run the git checkout command. Let’s switch to the new testing
branch:


  $ git checkout testing


This moves HEAD to point to the testing branch.




66

<!-- side 67 -->

Figure 14. HEAD points to the current branch

What is the significance of that? Well, let’s do another commit:


  $ vim test.rb
  $ git commit -a -m 'made a change'




Figure 15. The HEAD branch moves forward when a commit is made

This is interesting, because now your testing branch has moved forward, but your master branch
still points to the commit you were on when you ran git checkout to switch branches. Let’s switch
back to the master branch:


  $ git checkout master




                                                                                              67

<!-- side 68 -->

               git log doesn’t show all the branches all the time
               If you were to run git log right now, you might wonder where the "testing"
               branch you just created went, as it would not appear in the output.

               The branch hasn’t disappeared; Git just doesn’t know that you’re interested in that
              branch and it is trying to show you what it thinks you’re interested in. In other
               words, by default, git log will only show commit history below the branch you’ve
               checked out.

               To show commit history for the desired branch you have to explicitly specify it: git
               log testing. To show all of the branches, add --all to your git log command.




Figure 16. HEAD moves when you checkout

That command did two things. It moved the HEAD pointer back to point to the master branch, and it
reverted the files in your working directory back to the snapshot that master points to. This also
means the changes you make from this point forward will diverge from an older version of the
project. It essentially rewinds the work you’ve done in your testing branch so you can go in a
different direction.

               Switching branches changes files in your working directory
               It’s important to note that when you switch branches in Git, files in your working
              directory will change. If you switch to an older branch, your working directory
               will be reverted to look like it did the last time you committed on that branch. If Git
               cannot do it cleanly, it will not let you switch at all.


Let’s make a few changes and commit again:


  $ vim test.rb
  $ git commit -a -m 'made other changes'


Now your project history has diverged (see Divergent history). You created and switched to a
branch, did some work on it, and then switched back to your main branch and did other work. Both
of those changes are isolated in separate branches: you can switch back and forth between the



68

<!-- side 69 -->

branches and merge them together when you’re ready. And you did all that with simple branch,
checkout, and commit commands.




Figure 17. Divergent history

You can also see this easily with the git log command. If you run git log --oneline --decorate
--graph --all it will print out the history of your commits, showing where your branch pointers are
and how your history has diverged.


  $ git log --oneline --decorate --graph --all
  * c2b9e (HEAD, master) Made other changes
  | * 87ab2 (testing) Made a change
  |/
  * f30ab Add feature #32 - ability to add new formats to the central interface
  * 34ac2 Fix bug #1328 - stack overflow under certain conditions
  * 98ca9 initial commit of my project


Because a branch in Git is actually a simple file that contains the 40 character SHA-1 checksum of
the commit it points to, branches are cheap to create and destroy. Creating a new branch is as quick
and simple as writing 41 bytes to a file (40 characters and a newline).

This is in sharp contrast to the way most older VCS tools branch, which involves copying all of the
project’s files into a second directory. This can take several seconds or even minutes, depending on
the size of the project, whereas in Git the process is always instantaneous. Also, because we’re
recording the parents when we commit, finding a proper merge base for merging is automatically
done for us and is generally very easy to do. These features help encourage developers to create
and use branches often.

Let’s see why you should do so.



                                                                                                 69

<!-- side 70 -->

               Creating a new branch and switching to it at the same time
               It’s typical to create a new branch and want to switch to that new branch at the
              same time — this can be done in one operation with git                   checkout   -b
               <newbranchname>.


               From Git version 2.23 onwards you can use git switch instead of git checkout to:

                 • Switch to an existing branch: git switch testing-branch.
                • Create a new branch and switch to it: git switch -c new-branch. The -c flag
                   stands for create, you can also use the full flag: --create.

                 • Return to your previously checked out branch: git switch -.



Basic Branching and Merging
Let’s go through a simple example of branching and merging with a workflow that you might use in
the real world. You’ll follow these steps:

1. Do some work on a website.

2. Create a branch for a new user story you’re working on.

3. Do some work in that branch.

At this stage, you’ll receive a call that another issue is critical and you need a hotfix. You’ll do the
following:

1. Switch to your production branch.

2. Create a branch to add the hotfix.

3. After it’s tested, merge the hotfix branch, and push to production.

4. Switch back to your original user story and continue working.


Basic Branching

First, let’s say you’re working on your project and have a couple of commits already on the master
branch.




Figure 18. A simple commit history




70

<!-- side 71 -->

You’ve decided that you’re going to work on issue #53 in whatever issue-tracking system your
company uses. To create a new branch and switch to it at the same time, you can run the git
checkout command with the -b switch:


  $ git checkout -b iss53
  Switched to a new branch "iss53"


This is shorthand for:


  $ git branch iss53
  $ git checkout iss53




Figure 19. Creating a new branch pointer

You work on your website and do some commits. Doing so moves the iss53 branch forward,
because you have it checked out (that is, your HEAD is pointing to it):


  $ vim index.html
  $ git commit -a -m 'Create new footer [issue 53]'




                                                                                         71

<!-- side 72 -->

Figure 20. The iss53 branch has moved forward with your work

Now you get the call that there is an issue with the website, and you need to fix it immediately. With
Git, you don’t have to deploy your fix along with the iss53 changes you’ve made, and you don’t
have to put a lot of effort into reverting those changes before you can work on applying your fix to
what is in production. All you have to do is switch back to your master branch.

However, before you do that, note that if your working directory or staging area has uncommitted
changes that conflict with the branch you’re checking out, Git won’t let you switch branches. It’s
best to have a clean working state when you switch branches. There are ways to get around this
(namely, stashing and commit amending) that we’ll cover later on, in Stashing and Cleaning. For
now, let’s assume you’ve committed all your changes, so you can switch back to your master branch:


  $ git checkout master
  Switched to branch 'master'


At this point, your project working directory is exactly the way it was before you started working
on issue #53, and you can concentrate on your hotfix. This is an important point to remember:
when you switch branches, Git resets your working directory to look like it did the last time you
committed on that branch. It adds, removes, and modifies files automatically to make sure your
working copy is what the branch looked like on your last commit to it.

Next, you have a hotfix to make. Let’s create a hotfix branch on which to work until it’s completed:


  $ git checkout -b hotfix
  Switched to a new branch 'hotfix'
  $ vim index.html
  $ git commit -a -m 'Fix broken email address'
  [hotfix 1fb7853] Fix broken email address
   1 file changed, 2 insertions(+)




72

<!-- side 73 -->

Figure 21. Hotfix branch based on master

You can run your tests, make sure the hotfix is what you want, and finally merge the hotfix branch
back into your master branch to deploy to production. You do this with the git merge command:


  $ git checkout master
  $ git merge hotfix
  Updating f42c576..3a0874c
  Fast-forward
   index.html | 2 ++
   1 file changed, 2 insertions(+)


You’ll notice the phrase “fast-forward” in that merge. Because the commit C4 pointed to by the
branch hotfix you merged in was directly ahead of the commit C2 you’re on, Git simply moves the
pointer forward. To phrase that another way, when you try to merge one commit with a commit
that can be reached by following the first commit’s history, Git simplifies things by moving the
pointer forward because there is no divergent work to merge together — this is called a “fast-
forward.”

Your change is now in the snapshot of the commit pointed to by the master branch, and you can
deploy the fix.




                                                                                                73

<!-- side 74 -->

Figure 22. master is fast-forwarded to hotfix

After your super-important fix is deployed, you’re ready to switch back to the work you were doing
before you were interrupted. However, first you’ll delete the hotfix branch, because you no longer
need it — the master branch points at the same place. You can delete it with the -d option to git
branch:


  $ git branch -d hotfix
  Deleted branch hotfix (3a0874c).


Now you can switch back to your work-in-progress branch on issue #53 and continue working on it.


  $ git checkout iss53
  Switched to branch "iss53"
  $ vim index.html
  $ git commit -a -m 'Finish the new footer [issue 53]'
  [iss53 ad82d7a] Finish the new footer [issue 53]
  1 file changed, 1 insertion(+)




74

<!-- side 75 -->

Figure 23. Work continues on iss53

It’s worth noting here that the work you did in your hotfix branch is not contained in the files in
your iss53 branch. If you need to pull it in, you can merge your master branch into your iss53
branch by running git merge master, or you can wait to integrate those changes until you decide to
pull the iss53 branch back into master later.


Basic Merging

Suppose you’ve decided that your issue #53 work is complete and ready to be merged into your
master branch. In order to do that, you’ll merge your iss53 branch into master, much like you
merged your hotfix branch earlier. All you have to do is check out the branch you wish to merge
into and then run the git merge command:


  $ git checkout master
  Switched to branch 'master'
  $ git merge iss53
  Merge made by the 'recursive' strategy.
  index.html |    1 +
  1 file changed, 1 insertion(+)


This looks a bit different than the hotfix merge you did earlier. In this case, your development
history has diverged from some older point. Because the commit on the branch you’re on isn’t a
direct ancestor of the branch you’re merging in, Git has to do some work. In this case, Git does a
simple three-way merge, using the two snapshots pointed to by the branch tips and the common
ancestor of the two.




                                                                                                75

<!-- side 76 -->

Figure 24. Three snapshots used in a typical merge

Instead of just moving the branch pointer forward, Git creates a new snapshot that results from this
three-way merge and automatically creates a new commit that points to it. This is referred to as a
merge commit, and is special in that it has more than one parent.




Figure 25. A merge commit

Now that your work is merged in, you have no further need for the iss53 branch. You can close the
issue in your issue-tracking system, and delete the branch:


  $ git branch -d iss53



Basic Merge Conflicts

Occasionally, this process doesn’t go smoothly. If you changed the same part of the same file
differently in the two branches you’re merging, Git won’t be able to merge them cleanly. If your fix
for issue #53 modified the same part of a file as the hotfix branch, you’ll get a merge conflict that
looks something like this:




76

<!-- side 77 -->

  $ git merge iss53
  Auto-merging index.html
  CONFLICT (content): Merge conflict in index.html
  Automatic merge failed; fix conflicts and then commit the result.


Git hasn’t automatically created a new merge commit. It has paused the process while you resolve
the conflict. If you want to see which files are unmerged at any point after a merge conflict, you can
run git status:


  $ git status
  On branch master
  You have unmerged paths.
    (fix conflicts and run "git commit")

  Unmerged paths:
    (use "git add <file>..." to mark resolution)

      both modified:          index.html

  no changes added to commit (use "git add" and/or "git commit -a")


Anything that has merge conflicts and hasn’t been resolved is listed as unmerged. Git adds standard
conflict-resolution markers to the files that have conflicts, so you can open them manually and
resolve those conflicts. Your file contains a section that looks something like this:


  <<<<<<< HEAD:index.html
  <div id="footer">contact : email.support@github.com</div>
  =======
  <div id="footer">
   please contact us at support@github.com
  </div>
  >>>>>>> iss53:index.html


This means the version in HEAD (your master branch, because that was what you had checked out
when you ran your merge command) is the top part of that block (everything above the =======),
while the version in your iss53 branch looks like everything in the bottom part. In order to resolve
the conflict, you have to either choose one side or the other or merge the contents yourself. For
instance, you might resolve this conflict by replacing the entire block with this:


  <div id="footer">
  please contact us at email.support@github.com
  </div>


This resolution has a little of each section, and the <<<<<<<, =======, and >>>>>>> lines have been
completely removed. After you’ve resolved each of these sections in each conflicted file, run git add



                                                                                                   77

