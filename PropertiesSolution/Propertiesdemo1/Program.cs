using System;


namespace propertiesdemo1
{
    class Program
    {
        int _Custid;
        string _Custname;
        double _Balance;

        public int Custid        
        {
            get{ return _Custid;}
            set{ _Custid=value;}
        }
        public double Balance
        {

            get{ 
                    if(_Balance<0)
                    {
                        Console.WriteLine("you cannot get value more than you have in your account");
                        return 0;
                    }
                    else return _Balance;
                }

            set {
                    if(value<0) 
                        {
                           Console.WriteLine("cannot set value less than 0");
                         }
                else {
                    _Balance = value; 
                }
                }

        }
        public string Custname
        { 
            get{ return _Custname; } 
            set{ _Custname=value;} 
        }
        
        public static void Main()
        {
            Program program = new Program();
            
            program.Custid = 1;
            Console.WriteLine("custid: " + program.Custid);

            program.Custname = "rohith";
            Console.WriteLine("Custname: " + program.Custname);

            program.Balance = -100;
            Console.WriteLine("Balance: " + program.Balance); 

        }
    }
    
}