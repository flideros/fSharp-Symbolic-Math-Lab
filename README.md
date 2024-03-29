*** 3/7/23
I have decided to retire this repository. The dependancies have become too old to maintain. I will be starting a new coding and revisit the symbolic math code. I will also create a lab for the Wolfram Engine programs.
***
# fSharp-Symbolic-Math-Lab
Personal coding lab for studying computer mathematics in f#

This repository is what I use to learn how to code in F#. I started coding with the Symbolic Math library. At the time, I was reading a book about computer algebra, so codding Symbolic Math was a great way to follow along with the book.

Then, I decided to take it a step further and build a user interface to go along with Symbolic Math. After evaluating several options for coding the UI, I settled on WPF because I could code it all in a single language. You won't see a lot of Xaml in these projects. It's not that I don't like Xaml (I may use it in the future), it's just easier to code the UI in a single language.

To learn WPF, a started coding the Basic Calculator. I used the Calculator Walkthrough blog posts from Scott Wlaschin's website https://fsharpforfunandprofit.com/ as a starting point. Later, I added the Database Lab. This is a useful tool for interacting with a database.

My current focus is the Graphing Calculator. It is inspired by the graphing calculator from the WPF Sample Applications. This is the first project that I use Symbolic Math as the CAS. My plan is to finish all the functionality within the limits of the UI from the WPF Sample Applications, then move on to the next phase.

The code is very verbose (on purpose), but anyone with a math or computer background should be able to follow along with my logic. My hope is for people to use this repository as a way to learn math and code F# WPF applications. So, as I learn (and code), you will see this repository grow.

Sincerely,

FLideros

3/6/22 -- Added a skeleton domain model for materials to the Analysis Lab. I am going to implement a strain analysis for the truss system before I begin work on a material builder control. 

1/10/22 -- Started a new project called Analysis Lab. My intent is to factor the code basis (Truss Analysis in previous projects tab) into a more general purpose analysis tool. This project will continue to use the Wolfram engine.

7/21/21 -- If you want to run this repository, you will need the Wolfram Kernel. If you edit the Symbolic Math UI Program to exclude the Wolfram Display project, you should be able to run this repository with out the kernel. I am curently writing a truss analysis program and I intend to use the wolfram engine in it.
