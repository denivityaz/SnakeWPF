using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace snake_wpf
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Head = LoadImage("Head.png");
        public readonly static ImageSource Food = LoadImage("Food.png");
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");

        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri(@$"/Assets/{fileName}", UriKind.Relative));
        }
    } 
}

// Наработки для динамической замены цветов в зависимости от цвета сьеденной еды

//public static ImageSource Empty => LoadImage("Empty.png");
////public static ImageSource Food => FoodGreen;

//public static ImageSource Food => LoadRandomFoodImage();
//public static ImageSource DeadBody => LoadImage("DeadBody.png");
//public static ImageSource DeadHead => LoadImage("DeadHead.png");
//public static ImageSource Head => LoadImage("Head.png");

//public static ImageSource SnakeBodyOrange => LoadImageFromSnakeFolder("BodyOrange.png");
//public static ImageSource SnakeHeadOrange => LoadImageFromSnakeFolder("HeadOrange.png");
//public static ImageSource SnakeBodyGreen => LoadImageFromSnakeFolder("BodyGreen.png");
//public static ImageSource SnakeHeadGreen => LoadImageFromSnakeFolder("HeadGreen.png");

//public static ImageSource FoodOrange => LoadImage("FoodOrange.png");
//public static ImageSource FoodGreen => LoadImage("FoodGreen.png");

////public static ImageSource Empty => LoadImage("Empty.png");
////public static ImageSource SnakeBodyOrange => LoadImage("BodyOrange.png");
////public static ImageSource SnakeBodyGreen => LoadImage("BodyGreen.png");
////public static ImageSource Head => LoadImage("Head.png");
////public static ImageSource FoodOrange => LoadImage("FoodOrange.png");
////public static ImageSource FoodGreen => LoadImage("FoodGreen.png");
////public static ImageSource DeadBody => LoadImage("DeadBody.png");
////public static ImageSource DeadHead => LoadImage("DeadHead.png");


//// Загружает случайное изображение для еды
//private static ImageSource LoadRandomFoodImage()
//{
//    string[] foodColors = new string[] { "Orange", "Green" };
//    Random random = new Random();
//    string randomColor = foodColors[random.Next(foodColors.Length)];
//    string fileName = $"Food_{randomColor}.png";
//    return LoadImage(fileName);
//}

//// Загружает изображение для змеи в зависимости от цвета еды
//private static ImageSource LoadImageFromSnakeFolder(string fileName)
//{
//    string foodColor = (Images.Food == LoadImage("GreenFood.png")) ? "Green" : "Orange";
//    return LoadImage($"{foodColor}Snake/{fileName}");
//}

//public static ImageSource LoadImage(string fileName)
//{
//    return new BitmapImage(new Uri(@$"/Assets/{fileName}", UriKind.Relative));
//}