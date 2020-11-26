using System;
using System.Collections.Generic;
using System.Text;

namespace ThirdTask.GameDescription
{
	public class Pad
	{
		// все десятки считаются равными друг другу
		public int[] cards = new int[12] { 0, 0, 32, 32, 32, 32, 32, 32, 32, 32, 128, 32 }; // в игру??
												    
												   
		//public (string, int)[] cards_ = new (string, int)[12]; // сделать через кортежи - не прокатит из-за 10ок, делаем равнозначность/переписываем массив и рандом по хитрому
	}
}