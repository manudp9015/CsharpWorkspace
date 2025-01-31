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

        public Object this[String name]
        {
            get
            {
                if (name == "eno")
                    return eno;
                else if (name == "ename")
                    return ename;
                else if (name == "dname")
                    return dname;
                else if (name == "job")
                    return job;
                else if (name == "sal")
                    return sal;
                else if (name == "location")
                    return location;
                return null;
            }
            set
            {
                if (name == "eno")
                    eno = (int)value;
                else if (name == "ename")
                    ename = (string)value;
                else if (name == "dname")
                    dname = (string)value;
                else if (name == "job")
                    job = (string)job;
                else if (name == "sal")
                    sal = (double)value;
                else if (name == "location")
                    location = (string)value;

            }
        }
    }

}
