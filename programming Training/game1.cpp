#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int main()
{
  int i = 1;
  int mynumber;
  int randomnumber;
  srand(time(NULL));
  randomnumber = rand() % 101; //수의 범위
  while (i <= 10)
  {
    printf("%d번째로 추리할 숫자를 입력하세요: ", i);
    scanf_s("%d", &mynumber);
    if (mynumber == randomnumber)
    {
      printf("맞추셨습니다. 정답입니다!\\n");
      break;
    }
    else if (mynumber != randomnumber)
    {
      printf("틀리셨습니다.");
      if (mynumber < randomnumber)
      {
        printf("입력하신 수는 정답보다 작습니다.\\n");
      }
      else
      {
        printf("입력하신 수는 정답보다 큽니다.\\n");
      }
      if (i == 10 && mynumber != randomnumber)
      {
        printf("게임에서 패배하였습니다... You Lose \\n");
        break;
      }
      i++;
    }   
  }
}
