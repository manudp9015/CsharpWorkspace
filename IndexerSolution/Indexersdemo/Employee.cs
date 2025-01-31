using System;

namespace Indexerdemo
{

    class Employee
    {
        int eno;
        double sal;
        String ename, location, job, dname;
        public Employee(int eno, string ename, string dname, string job, double sal, string location)
        {
            this.eno = eno;
            this.ename = ename;
            this.sal = sal;
            this.location = location;
            this.job = job;
            this.dname = dname;

        }

        public Object this[int index]
        {
            get
            {
                if (index == 0)
                    return eno;
                else if (index == 1)
                    return ename;
                else if (index == 2)    
                    return dname;
                else if (index == 3)    
                    return job;
                else if (index == 4)    
                    return sal;
                else if (index == 5)    
                    return location;
                return null;
            }
            set
            {
                if (index == 0)
                     eno=(int)value;
                else if (index == 1)
                     ename=(string)value;
                else if (index == 2)
                     dname=(string) value;
                else if (index == 3)
                     job=(string)job;
                else if (index == 4)
                     sal=(double)value;
                else if (index == 5)
                     location=(string)value;
                 
            }
        }
    }

}
