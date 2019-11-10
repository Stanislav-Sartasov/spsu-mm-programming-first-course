#include <stdio.h>
#include <stdlib.h>

typedef struct element
{
   int data;
   struct element *next;
   struct element *prev;
} list;


struct long_number
{
   void (*append) (list**, int);
   void (*multiply_hex) (list*, int);
   void (*print_hex) (list*);
   list *number;
};

list *get_last(list *head)
{
   while (head -> next)
       head = head -> next;
   return head;
}

void append(list **head, int a)
{
   if (*head == NULL)
   {
       *head = (list*) malloc(sizeof(list));
       (*head) -> data = a;
       (*head) -> next = NULL;
       (*head) -> prev =NULL;
   }
   else
   {
       list *last = get_last(*head);
       list *new_last = (list*) malloc(sizeof(list));
       new_last -> data = a;
       new_last -> next = NULL;
       new_last -> prev = last;
       last -> next = new_last;
   }
}

void multiply_hex(list *head, int a)
{
    int overflow = (head -> data) * a / 16;
    head -> data = (head -> data) * a % 16;
    while (head -> next)
    {
        head = head -> next;
        int x = (head -> data) * a + overflow;
        head -> data = x % 16;
        overflow = x / 16;
    }
    if (overflow > 0)
        append(&head, overflow);
}

void print_hex(list *head)
{
    list *last = get_last(head);
    printf("%X", last -> data);
    while (last -> prev)
    {
        last = last -> prev;
        printf("%X", last -> data);
    }
}

int main()
{
    struct long_number a = {&append, &multiply_hex, &print_hex, NULL};
    a.append(&(a.number), 1);
    for (int i = 0; i < 5000; i++)
        a.multiply_hex(a.number, 3);
    printf("Answer:\n");
    a.print_hex(a.number);
    printf("\nEnd.");
    return 0;
}

