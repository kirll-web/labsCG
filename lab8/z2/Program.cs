using z1;

namespace z1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Form1 form1 = new Form1();
            Application.Run(form1);
        }
    }
}
