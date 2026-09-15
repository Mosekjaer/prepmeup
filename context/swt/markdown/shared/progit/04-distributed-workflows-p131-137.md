---
title: "Pro Git — Distributed Workflows - Small Private Teams"
source: "progit.pdf"
modul: "Supplerende laesning (uddrag)"
pages: "131-137"
type: "bog-uddrag"
pensum: false
---

# Pro Git — Distributed Workflows - Small Private Teams (bogens s. 131-137)

<!-- side 131 -->

messages should start with a single line that’s no more than about 50 characters and that describes
the changeset concisely, followed by a blank line, followed by a more detailed explanation. The Git
project requires that the more detailed explanation include your motivation for the change and
contrast its implementation with previous behavior — this is a good guideline to follow. Write your
commit message in the imperative: "Fix bug" and not "Fixed bug" or "Fixes bug." Here is a template
you can follow, which we’ve lightly adapted from one originally written by Tim Pope:


  Capitalized, short (50 chars or less) summary

  More detailed explanatory text, if necessary. Wrap it to about 72
  characters or so. In some contexts, the first line is treated as the
  subject of an email and the rest of the text as the body. The blank
  line separating the summary from the body is critical (unless you omit
  the body entirely); tools like rebase will confuse you if you run the
  two together.

  Write your commit message in the imperative: "Fix bug" and not "Fixed bug"
  or "Fixes bug." This convention matches up with commit messages generated
  by commands like git merge and git revert.

  Further paragraphs come after blank lines.

  - Bullet points are okay, too

  - Typically a hyphen or asterisk is used for the bullet, followed by a
    single space, with blank lines in between, but conventions vary here

  - Use a hanging indent


If all your commit messages follow this model, things will be much easier for you and the
developers with whom you collaborate. The Git project has well-formatted commit messages — try
running git log --no-merges there to see what a nicely-formatted project-commit history looks like.

              Do as we say, not as we do.
              For the sake of brevity, many of the examples in this book don’t have nicely-

             formatted commit messages like this; instead, we simply use the -m option to git
              commit.

              In short, do as we say, not as we do.


Private Small Team

The simplest setup you’re likely to encounter is a private project with one or two other developers.
“Private,” in this context, means closed-source — not accessible to the outside world. You and the
other developers all have push access to the repository.

In this environment, you can follow a workflow similar to what you might do when using
Subversion or another centralized system. You still get the advantages of things like offline



                                                                                                131

<!-- side 132 -->

committing and vastly simpler branching and merging, but the workflow can be very similar; the
main difference is that merges happen client-side rather than on the server at commit time. Let’s
see what it might look like when two developers start to work together with a shared repository.
The first developer, John, clones the repository, makes a change, and commits locally. The protocol
messages have been replaced with … in these examples to shorten them somewhat.


  # John's Machine
  $ git clone john@githost:simplegit.git
  Cloning into 'simplegit'...
  ...
  $ cd simplegit/
  $ vim lib/simplegit.rb
  $ git commit -am 'Remove invalid default value'
  [master 738ee87] Remove invalid default value
   1 files changed, 1 insertions(+), 1 deletions(-)


The second developer, Jessica, does the same thing — clones the repository and commits a change:


  # Jessica's Machine
  $ git clone jessica@githost:simplegit.git
  Cloning into 'simplegit'...
  ...
  $ cd simplegit/
  $ vim TODO
  $ git commit -am 'Add reset task'
  [master fbff5bc] Add reset task
   1 files changed, 1 insertions(+), 0 deletions(-)


Now, Jessica pushes her work to the server, which works just fine:


  # Jessica's Machine
  $ git push origin master
  ...
  To jessica@githost:simplegit.git
      1edee6b..fbff5bc master -> master


The last line of the output above shows a useful return message from the push operation. The basic
format is <oldref>..<newref> fromref → toref, where oldref means the old reference, newref means
the new reference, fromref is the name of the local reference being pushed, and toref is the name of
the remote reference being updated. You’ll see similar output like this below in the discussions, so
having a basic idea of the meaning will help in understanding the various states of the repositories.
More details are available in the documentation for git-push.

Continuing with this example, shortly afterwards, John makes some changes, commits them to his
local repository, and tries to push them to the same server:




132

<!-- side 133 -->

  # John's Machine
  $ git push origin master
  To john@githost:simplegit.git
   ! [rejected]        master -> master (non-fast forward)
  error: failed to push some refs to 'john@githost:simplegit.git'


In this case, John’s push fails because of Jessica’s earlier push of her changes. This is especially
important to understand if you’re used to Subversion, because you’ll notice that the two developers
didn’t edit the same file. Although Subversion automatically does such a merge on the server if
different files are edited, with Git, you must first merge the commits locally. In other words, John
must first fetch Jessica’s upstream changes and merge them into his local repository before he will
be allowed to push.

As a first step, John fetches Jessica’s work (this only fetches Jessica’s upstream work, it does not yet
merge it into John’s work):


  $ git fetch origin
  ...
  From john@githost:simplegit
   + 049d078...fbff5bc master          -> origin/master


At this point, John’s local repository looks something like this:




Figure 57. John’s divergent history

Now John can merge Jessica’s work that he fetched into his own local work:




                                                                                                    133

<!-- side 134 -->

  $ git merge origin/master
  Merge made by the 'recursive' strategy.
   TODO |    1 +
   1 files changed, 1 insertions(+), 0 deletions(-)


As long as that local merge goes smoothly, John’s updated history will now look like this:




Figure 58. John’s repository after merging origin/master

At this point, John might want to test this new code to make sure none of Jessica’s work affects any
of his and, as long as everything seems fine, he can finally push the new merged work up to the
server:


  $ git push origin master
  ...
  To john@githost:simplegit.git
      fbff5bc..72bbc59 master -> master


In the end, John’s commit history will look like this:




Figure 59. John’s history after pushing to the origin server



134

<!-- side 135 -->

In the meantime, Jessica has created a new topic branch called issue54, and made three commits to
that branch. She hasn’t fetched John’s changes yet, so her commit history looks like this:




Figure 60. Jessica’s topic branch

Suddenly, Jessica learns that John has pushed some new work to the server and she wants to take a
look at it, so she can fetch all new content from the server that she does not yet have with:


  # Jessica's Machine
  $ git fetch origin
  ...
  From jessica@githost:simplegit
      fbff5bc..72bbc59 master            -> origin/master


That pulls down the work John has pushed up in the meantime. Jessica’s history now looks like this:




Figure 61. Jessica’s history after fetching John’s changes

Jessica thinks her topic branch is ready, but she wants to know what part of John’s fetched work she
has to merge into her work so that she can push. She runs git log to find out:


  $ git log --no-merges issue54..origin/master
  commit 738ee872852dfaa9d6634e0dea7a324040193016
  Author: John Smith <jsmith@example.com>
  Date:   Fri May 29 16:01:27 2009 -0700

      Remove invalid default value


The issue54..origin/master syntax is a log filter that asks Git to display only those commits that are
on the latter branch (in this case origin/master) that are not on the first branch (in this case


                                                                                                  135

<!-- side 136 -->

issue54). We’ll go over this syntax in detail in Commit Ranges.

From the above output, we can see that there is a single commit that John has made that Jessica has
not merged into her local work. If she merges origin/master, that is the single commit that will
modify her local work.

Now, Jessica can merge her topic work into her master branch, merge John’s work (origin/master)
into her master branch, and then push back to the server again.

First (having committed all of the work on her issue54 topic branch), Jessica switches back to her
master branch in preparation for integrating all this work:


  $ git checkout master
  Switched to branch 'master'
  Your branch is behind 'origin/master' by 2 commits, and can be fast-forwarded.


Jessica can merge either origin/master or issue54 first — they’re both upstream, so the order doesn’t
matter. The end snapshot should be identical no matter which order she chooses; only the history
will be different. She chooses to merge the issue54 branch first:


  $ git merge issue54
  Updating fbff5bc..4af4298
  Fast forward
   README           |    1 +
   lib/simplegit.rb |    6 +++++-
   2 files changed, 6 insertions(+), 1 deletions(-)


No problems occur; as you can see it was a simple fast-forward merge. Jessica now completes the
local merging process by merging John’s earlier fetched work that is sitting in the origin/master
branch:


  $ git merge origin/master
  Auto-merging lib/simplegit.rb
  Merge made by the 'recursive' strategy.
   lib/simplegit.rb |    2 +-
   1 files changed, 1 insertions(+), 1 deletions(-)


Everything merges cleanly, and Jessica’s history now looks like this:




136

<!-- side 137 -->

Figure 62. Jessica’s history after merging John’s changes

Now origin/master is reachable from Jessica’s master branch, so she should be able to successfully
push (assuming John hasn’t pushed even more changes in the meantime):


  $ git push origin master
  ...
  To jessica@githost:simplegit.git
      72bbc59..8059c15 master -> master


Each developer has committed a few times and merged each other’s work successfully.




Figure 63. Jessica’s history after pushing all changes back to the server

That is one of the simplest workflows. You work for a while (generally in a topic branch), and
merge that work into your master branch when it’s ready to be integrated. When you want to share
that work, you fetch and merge your master from origin/master if it has changed, and finally push
to the master branch on the server. The general sequence is something like this:




                                                                                              137

