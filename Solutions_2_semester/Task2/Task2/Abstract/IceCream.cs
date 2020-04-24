using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IceCream
{
    public abstract class AbstractIceCream
    {        
        public enum Type
        {
            creamy,
            strawberry,
            chocolate,
            pistachio,
            caramel,
            special
        }
        public enum Innings
        {
            briquette,
            onStick,
            ball,
            withWaffle,
            inTheHorn
        }
        protected const int StdServingSize = 70;

        public Type type { get; protected set; }
        public Innings innings { get; protected set; }
        public int count { get; protected set; }    //if briquette or withWaffle: grams; if ball or inTheHorn or onStick: count of portions

        public virtual string GetRecipe()
        {
            bool isSpecial = (type == Type.special);
            if (count <= 0)
                return null;
            double coefficient;
            switch (innings)
            {
                case Innings.ball:
                case Innings.inTheHorn:
                case Innings.onStick:
                    coefficient = count;
                    break;
                default:
                    coefficient = count / StdServingSize;
                    break;
            }
            string useCoeff(double x)
            {
                int a = (int)(x * coefficient);
                if (a == 0)
                    a = 1;
                if (isSpecial)
                    return $"0x{a:X} ";
                else
                    return a.ToString();
            }
            string recipe = "Рецепт ";
            switch (type)
            {
                case Type.creamy:
                    recipe += "сливочного ";
                    break;
                case Type.chocolate:
                    recipe += "шоколодного ";
                    break;
                case Type.strawberry:
                    recipe += "клубничного ";
                    break;
                case Type.caramel:
                    recipe += "карамельного ";
                    break;
                case Type.pistachio:
                    recipe += "фисташкового ";
                    break;
                case Type.special:
                    recipe += "специального ";
                    break;
            }
            switch (innings)
            {
                case Innings.briquette:
                    recipe += "брикета мороженного ";
                    break;
                case Innings.ball:
                    recipe += " мороженного в шариках, " + count.ToString() + "шт.\n";
                    break;
                case Innings.inTheHorn:
                    recipe += "мороженного в рожке, ";
                    if (isSpecial)
                        recipe += $"0x{count:X} ";
                    else
                        recipe += count.ToString();
                    recipe += "шт.\n";
                    break;
                case Innings.withWaffle:
                    recipe += "мороженного с вафлей ";
                    break;
                case Innings.onStick:
                    recipe += "мороженного на палочке, " + count.ToString() + "шт.\n";
                    break;
            }
            if (innings != Innings.ball && innings != Innings.inTheHorn && innings != Innings.onStick)
                recipe += count.ToString() + "г.\n";
            recipe += "\nПонадобится\n\nМороженное:\n";
            recipe += "\tСливки 33% жирности - " + useCoeff(35) + "мл;\n";
            recipe += "\tМолоко - " + useCoeff(25) + "мл;\n";
            recipe += "\tСахар - " + useCoeff(10) + "г\n";
            recipe += "\tЖелтки яичные - " + useCoeff(0.3) + "шт;\n";
            switch (type)
            {
                case Type.strawberry:
                    recipe += "\tКлубника - ";
                    break;
                case Type.chocolate:
                    recipe += "\tКакао - ";
                    break;
                case Type.caramel:
                    recipe += "\tКарамель - ";
                    break;
                case Type.pistachio:
                    recipe += "\tФисташки - ";
                    break;
                case Type.special:
                    recipe += "\tРаспечатанный на съедобной бумаге девственный код джунов - ";
                    break;
            }
            if (type == Type.creamy)
                recipe += "\tВанилин - по вкусу";
            else
                recipe += useCoeff(20) + "г";

            if (type == Type.special)
                recipe += ";\n\tШоколадные крошки из под клавиатуры - по вкусу";

            if (innings == Innings.onStick)
                recipe += ";\n\tПалочка для мороженного - " + count.ToString() + "шт.";

            if (innings == Innings.inTheHorn || innings == Innings.withWaffle)
            {
                coefficient /= 15;
                recipe += "\n\nВафля:\n";
                recipe += "\tСливочное масло — " + useCoeff(150) + "г;\n";
                recipe += "\tСахар  — " + useCoeff(250) + "г;\n";
                recipe += "\tЯйца  — " + useCoeff(4) + "шт;\n";
                recipe += "\tМука  — " + useCoeff(200) + "г;\n";
                recipe += "\tСоль - по вкусу";
            }

            recipe += "\n\nИзготовление\n\nМороженное:\n";
            recipe += "Желтки растираем с сахаром и ванилином.\n";
            recipe += "Молоко доводим до кипения и снимаем с огня. Аккуратно вводим в молоко желтковую массу,\nперемешиваем. Ставим на самый малый огонь и, взбивая, доводим до загустения.\nНе кипятим! Массу остужаем до комнатной температуры.\n";
            switch (type)
            {
                case Type.caramel:
                    recipe += "Добавляем в молочную массу карамель.\n";
                    break;
                case Type.chocolate:
                    recipe += "Добавляем в молочную массу какао.\n";
                    break;
                case Type.pistachio:
                    recipe += "Измельчить фисташки до состояния муки. Добавляем фисташки в молочную массу.\n";
                    break;
                case Type.strawberry:
                    recipe += "Протираем ягоду в молочную массу.\n";
                    break;
                case Type.special:
                    recipe += "Под звуки бубна и работающего жёсткого диска крошим в молочную массу\nраспечатанный на съедобной бумаге девственный код джунов. Добавляем шоколадные крошки.\n";
                    break;
            }
            recipe += "Охлажденные сливки взбиваем до устойчивых пиков.\n";
            recipe += "Вводим в сливки остывшую молочно-желтковую массу. ";
            if (type == Type.special)
                recipe += "Перемешиваем смесь 42 раза по двоичному\nкоду 42 (0 - вправо, 1 - влево), при этом  устанавливая Ubuntu 14.04 с докером mysql на нее.\nПри прерывании установки из-за несовместимости железа,\nвыливаем молочную массу и начинаем приготовление мороженного заново,\nпредварительно отформатировав жёсткий диск для повторной установки Ubontu.\n";
            else
                recipe += "Хорошо перемешиваем.\n";
            recipe += "Выкладываем в формы и отправляем пломбир в морозильную камеру.\nЧерез час достаем, перемешиваем миксером и отправляем мороженое обратно в морозильник\nдо полного застывания";

            if (innings == Innings.onStick)
                recipe += ", вставить в центр формы палочку для мороженного.";
            else
                recipe += ".";

            if (innings == Innings.ball)
                recipe += "\nПосле полного застывания сформировать из мороженного шарики, " + count.ToString() + "шт.";


            if (innings == Innings.inTheHorn || innings == Innings.withWaffle)
            {
                recipe += "\n\nВафля:\n";
                recipe += "Смешиваем растопленное сливочное масло, сахар, яйца и муку. Добавляем щепотку соли и\nзамешиваем тесто без комков. По консистенции оно должно получиться как сметана.\nДля приготовления понадобится вафельница.\n";
                recipe += "Наливаем немного теста на раскаленную основу вафельницы и выпекаем.";
                if (innings == Innings.inTheHorn)
                    recipe += "\nОткрыв крышку, нужно быстро завернуть рожок, пока он не застыл и не стал ломким.";
            }

            if (type == Type.special && innings == Innings.inTheHorn)
            {
                recipe += "\n\nРасположите мороженное на рожке как показано на картинке:\n\n";
                recipe += @"
                         .mmMMMMMMm.
                         MMMMMMMMMMNI
                        MMMMMMMMMMMMM
                        MMMMMMMMMMMM
                        !MMMMMMMMMMM
                        ! MMMMMMMM.
                        . !MM\!\!\!
                         .!\M\!\!\!.
                         !\!\!\!\!\!
                        .!\!\!\!\!\!.
                        !\!\!\!\!\!\!
                       .!\!\!\!\!\!\!.
                       !\!\!\!\!\!\!\!
                      .!\!\!\!\!\!\!\!.
                      !\!\!\!\!\!\!\!\!";
            }
            return recipe;
        }
    }
}