#include <stdio.h>

#define _CRT_SECURE_WARNINGS

#define COUNT_CURRENT_MONEY onePennies + twoPences * 2 + fivePences * 5 + tenPences * 10 + twentyPences * 20 + fiftyPences * 50 + onePounds * 100 + twoPounds * 200


int main() {
	int ammount = 0;


	printf("Enter the ammount of money: ");
	// Takes the user's input.
	do {
		scanf_s("%d", &ammount);
	} while (ammount <= 0);


	int onePennies = 0,
		twoPences = 0,
		fivePences = 0,
		tenPences = 0,
		twentyPences = 0,
		fiftyPences = 0,
		onePounds = 0,
		twoPounds = 0;

	int count = 0;

	int currentMoney = COUNT_CURRENT_MONEY;
	for (int twoPounds = 0; COUNT_CURRENT_MONEY <= ammount; twoPounds++){
		for (int onePounds = 0; COUNT_CURRENT_MONEY <= ammount; onePounds++) {
			currentMoney = COUNT_CURRENT_MONEY;
			for (int fiftyPences = 0; COUNT_CURRENT_MONEY <= ammount; fiftyPences++) {
				currentMoney = COUNT_CURRENT_MONEY;
				for (int twentyPences = 0; COUNT_CURRENT_MONEY <= ammount; twentyPences++) {
					currentMoney = COUNT_CURRENT_MONEY;
					for (int tenPences = 0; COUNT_CURRENT_MONEY <= ammount; tenPences++) {
						currentMoney = COUNT_CURRENT_MONEY;
						for (int fivePences = 0; COUNT_CURRENT_MONEY <= ammount; fivePences++) {
							currentMoney = COUNT_CURRENT_MONEY;
							for (int twoPences = 0; COUNT_CURRENT_MONEY <= ammount; twoPences++) {
								currentMoney = COUNT_CURRENT_MONEY;
								for (int onePennies = 0; COUNT_CURRENT_MONEY <= ammount; onePennies++) {
									currentMoney = COUNT_CURRENT_MONEY;
									if (currentMoney == ammount) {
										count++;
									}
								}
								onePennies = 0;
							}
							twoPences = 0;
						}
						fivePences = 0;
					}
					tenPences = 0;
				}
				twentyPences = 0;
			}
			fiftyPences = 0;
		}
		onePounds = 0;
	}


	printf("the number of ways in which this amount can be dialed using any number of any English coins is %d", count);

	return 0;
}