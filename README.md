# Cryptography_FeigeFiatShamirIdentification_IvanovA
Realization of Fiege-Fiat-Shamir identification algorithm by me

- How to use it? 

This algorithm is able to work in a short time with numbers up to 2^1024 (10^309). It is recommended to set the number of checks (Acc) equal to the square root of the specified number (For 10^309 - 1024). 
This is necessary for the correct operation of the simplicity test, which is made according to the Miller-Rabin algorithm. For it's work just follow the necessary steps indicated on the buttons (Number).
You may set necessary count of t rounds and necessary k(eys) amount. 
You can also choose working mode. In Handle mode you need to do all the steps by yourself, in auto, after pressing '4) Start' button, program will make process of identificaton by itself.
Moreover, you can uncheck checkbox 'More information' to get less information about working process.
If you chose the number of rounds T greater than one, do not forget to re-pass all the steps starting from the 6th point to finally unlock 'Final) Result after t rounds' button.
