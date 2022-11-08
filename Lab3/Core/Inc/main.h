/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.h
  * @brief          : Header for main.c file.
  *                   This file contains the common defines of the application.
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2021 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */
/* USER CODE END Header */

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MAIN_H
#define __MAIN_H

#ifdef __cplusplus
extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f1xx_hal.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "stdlib.h"
#include "stdbool.h"
/* USER CODE END Includes */

/* Exported types ------------------------------------------------------------*/
/* USER CODE BEGIN ET */

/* USER CODE END ET */

/* Exported constants --------------------------------------------------------*/
/* USER CODE BEGIN EC */

/* USER CODE END EC */

/* Exported macro ------------------------------------------------------------*/
/* USER CODE BEGIN EM */

/* USER CODE END EM */

/* Exported functions prototypes ---------------------------------------------*/
void Error_Handler(void);

/* USER CODE BEGIN EFP */

/* USER CODE END EFP */

/* Private defines -----------------------------------------------------------*/
#define B1_Pin GPIO_PIN_1
#define B1_GPIO_Port GPIOA
#define B1_EXTI_IRQn EXTI1_IRQn
#define B2_Pin GPIO_PIN_4
#define B2_GPIO_Port GPIOA
#define B2_EXTI_IRQn EXTI4_IRQn
#define B3_Pin GPIO_PIN_0
#define B3_GPIO_Port GPIOB
#define B3_EXTI_IRQn EXTI0_IRQn
#define CLK_Pin GPIO_PIN_8
#define CLK_GPIO_Port GPIOA
#define SDI_Pin GPIO_PIN_9
#define SDI_GPIO_Port GPIOA
#define LCH_Pin GPIO_PIN_5
#define LCH_GPIO_Port GPIOB
/* USER CODE BEGIN Private defines */
extern void displayPart(uint8_t digit);
extern void displayString(int8_t digits[4]);
extern void displayGeneration();
	
extern bool isStarted;
extern bool isGenerated;
	
extern uint8_t shiftNumber;
extern uint8_t spaceNumber;
extern uint8_t dotNumber;
extern uint8_t hexDigits[10];

extern int8_t outputDigits[4];

extern uint8_t generatedSize;
extern int8_t randomGenerated[28];

extern uint8_t speedMode;
extern uint8_t speedCount;
extern int speedStates[4];
/* USER CODE END Private defines */

#ifdef __cplusplus
}
#endif

#endif /* __MAIN_H */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
