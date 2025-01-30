
using System;


namespace Abstractdemo
{
    abstract class Figure
    {
        public double height, width, radius;
        public const float pi = 3.14f;
        abstract public  double GetArea();
    }
    class Rectangle : Figure
    {
        public Rectangle(double height, double width)
        {
            this.height = height;
            this.width = width;
        }
        public override double GetArea()
        {
            return height * width;

        }
    }
    class Circle : Figure
    {
        public Circle(double radius)
        {
            this.radius = radius;        
        }

        public override double GetArea()
        {
           return radius*radius*pi;
        }
    }
    class Triangle : Figure
    {
        public Triangle(double height,double width)
        {
            this.height = height;
            this.width = width;
        }
        public override double GetArea()
        {
            return (1 / 2.0 )* width * height;
        }
    }
    class MainFigure
    {
        public static void Main()
        {
            Circle circle=new Circle(10);
            Rectangle rectangle = new Rectangle(10, 10);
            Triangle triangle = new Triangle(10, 10);

            
            Console.WriteLine("circle: "+ Math.Round(circle.GetArea()));
            Console.WriteLine("triangle : " + triangle.GetArea());
            Console.WriteLine("rectangle: " + rectangle.GetArea());

              
        }
    }
}