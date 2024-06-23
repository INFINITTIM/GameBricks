using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//для таймера
using System.Windows.Threading;

namespace LAB3._0
{
    public partial class MainWindow : Window
    {
        Random random = new Random();
        private int n = 5;
        private int lineCount = 0;
        private bool canDelete = false;
        private int go = 0;

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            WindowState = WindowState.Maximized;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CreateRectangles(1);
        }
        
        //создаем наши линии (прямоугольники) в программе
        private void CreateRectangles(int nRect)
        {
            //если игра может еще не закончена то
            if (go == 0)
            {
                //создаем нужно количество прямоугольников
                for (int i = 0; i < nRect; i++)
                {
                    Rectangle rect = new Rectangle();

                    int orientation = random.Next(2);

                    if (orientation == 0)
                    {
                        rect.Width = 300;
                        rect.Height = 100;
                        rect.HorizontalAlignment = HorizontalAlignment.Left;
                        rect.VerticalAlignment = VerticalAlignment.Top;
                    }
                    else
                    {
                        rect.Width = 100;
                        rect.Height = 300;
                        rect.HorizontalAlignment = HorizontalAlignment.Left;
                        rect.VerticalAlignment = VerticalAlignment.Top;
                    }

                    //задаются рандомные координаты линит
                    int x = random.Next((int)(this.Width - rect.Width));
                    int y = random.Next((int)(this.Height - rect.Height));

                    rect.Margin = new Thickness(x, y, 0, 0);

                    //рисуем обводку и сам прямоугольник
                    rect.Stroke = Brushes.Black;
                    byte r = (byte)random.Next(0, 255);
                    byte g = (byte)random.Next(0, 255);
                    byte b = (byte)random.Next(0, 255);
                    rect.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));

                    lineCount++;

                    rect.MouseDown += Rect_MouseDown;

                    GridOne.Children.Add(rect);

                    //когда линий стало n можно их удалять
                    if (lineCount == n) canDelete = true;

                    FailAndVictory();
                }
            }
            else
            {
                return;
            }
        }

        //окно с победой или проигрышем
        private void FailAndVictory()
        {
            if(lineCount == 0)
            {
                go = 1;
                MessageBox.Show("You win");
            }
            else if (lineCount >= n*2)
            {
                go = -1;
                MessageBox.Show("You lose");
            }
        }


        private void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            Rect clickedRect = new Rect(rect.Margin.Left, rect.Margin.Top, rect.Width, rect.Height);
            //каждому нашему прямоугольнику присваиваем индекс
            int clickedRectIndex = GridOne.Children.IndexOf(rect);
            
            //проверяем все последующие
            for (int i = clickedRectIndex + 1; i < GridOne.Children.Count; i++)
            {
                //проверяем на перекрытия каждый наш и'тый прямоугольник создавая объект otherrect который являются областью прямоугольники и сравниваем перекрывает он или нет
                Rectangle otherRect = (Rectangle)GridOne.Children[i];
                Rect otherRectArea = new Rect(otherRect.Margin.Left, otherRect.Margin.Top, otherRect.Width, otherRect.Height);

                if (clickedRect.IntersectsWith(otherRectArea))
                {
                    // Если прямоугольники перекрываются, прерываем цикл
                    return;
                }
            }

            //если можно удалить то удаляем линию и убираем одну единичку из количества
            if (canDelete)
            {
                GridOne.Children.Remove(rect);
                lineCount--;
            }
            //вызываем окно показывывающее победили мы или проиграли
            FailAndVictory();
        }
    }
}