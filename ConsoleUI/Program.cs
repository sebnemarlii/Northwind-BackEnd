using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductTest();
            //CategoryTest();

        }

        private static void CategoryTest()
        {
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
            foreach (var category in categoryManager.GetAll().Data)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDal(),new CategoryManager(new EfCategoryDal()));
            var result = productManager.GetProductDetails();
            if (result.Success==true)
            {
                foreach (var product in result.Data)
                {
                    Console.WriteLine(product.ProductName + "/" + product.CategoryName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }



        }
    }
}


/*
 ASSIGNMENT-1 REPORT
CONTENT LIST
   1) Install Linux Distributions.
2) Install Git and Set Github account.
3) Install GCC, Write a simple “Hello Word” program and Write a simple Makefile.

 	Firstly, I did research on what Linux distributions are and which are more useful for me. There are a lot of Linux distributions. For example: Ubuntu, Kali Linux, Linux Mint, Debian. The next step was to decide which Linux distribution I should install. It was a bit of challenge to pick one of them. I chose ubuntu because it is simple to install, has a built-in firewall, virus protection method, freedom to customize your system and some software comes pre-installed for basic use. I learned that I could install Linux with USB or virtual machine in my research. It was a better idea for me to set up a Linux distribution with USB. I would be able to use my Linux distribution on any computer with just USB without carrying my computer. After a lot of videos and research, I decided to install a Linux distribution with USB. Then, I downloaded ubuntu and the rufus that application required for USB installation. But I could not install the Linux distribution with USB. For some reason something went wrong. So, I tried to install ubuntu with the virtual machine. I downloaded the required virtual machine. I completed the setup thanks to a few videos. 

Secondly, At the end of my research and the videos I watched, I first wrote the "apt-get install git-all" command required to install git to the terminal of my Linux distribution and checked the git version with the "git --version" command. Then we configure github with the commands "git config --global user.name" your_name "" and "git config --global user.email" email_adres "". Then I created a file called git_workspace with the command "mkdir git_workspace". I cloned my git repository by adding the url in the 'clone with http' section of the repository on github to the end of the "git clone" command.
Thirdly, I installed GCC with the command "sudo apt install build-essential". After, I created a C file where I’ll write my code with the command "touch HelloWorld.c". At first, I had a hard time writing a makefile because I did not understand what to do. After a few videos, I opened my terminal in my cloned github repository and created my makefile file with the "touch Makefile" command. I wrote it as follows: [task:] later I went to the next line and clicked tab and typed [command]. Then I typed 'make' in the terminal and made it desired. Finally, I pushed these things to github one by one and commit it with the command "git add file" and "git commit -m" message "file".

 */