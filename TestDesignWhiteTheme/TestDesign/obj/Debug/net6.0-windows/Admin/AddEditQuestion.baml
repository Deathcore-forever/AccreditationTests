   M S B A M L   `   `   `  ����M  IAccreditationTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TestDesign
TestDesign  5   	        TestDesign.AddEditQuestion   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TestDesign     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5	   	   $	��700��5
   	   $	��800��6   $��Consolas)�6"   .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   6*   �� 5      $��2* �6   6*   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      $��* �6   6)   .+�5   
   ]� $��0��6   ���#73B8CD.+�5      �� -   5         ��Name$ WindowHeader��5      $��Center=�6.   $��Center=�6K   
�������$
��24px(�6y   $
��Bold"�6�   6�   5      � 5   
   $��10q�6   $��1��6    .+�5      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   Вопрос:6]   �� 5      -   5         ��Name$ questionText��5      $
��20px(�6,    �� TextWrapping$
 Wrap=�6<   $��50��6P   $	��744��6\   6k   5      � 5!   
   -   5         �Name$ testsListContainer��5!      $��10 5 0 0q�60   $��2��6B   .+�5"      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $��	Тест:��6T   6c   �� 5#      -   5         ��Name$ 	testsList��5#   D   
�������$��0 0 0 2q�6*   $��250px��6o   $��30 0 0 0q�6}   ��6W   �� ��5$      �� 5%      .R�5&      �� $��NoWrap=�6$   $
��20px(�6:   ���#317F97��6_   �� *Name+6w   5'      5)      5*      � 5,   
   $��10 5 0 0q�6   $��3��6&   .+�5-      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $��Ответ 1:��6T   6f   �� 5.      -   5        $ response1Text��5.      $
��20px(�6-   $
��30px��6=   $	��654��6K   $��0 0 0 2q�6W   5/      � 51   
   $��10 5 0 0q�6   $��4��6&   .+�52      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $��Ответ 2:��6T   6f   �� 53      -   5        $ response2Text��53      $
��20px(�6-   $��0 0 0 1q�6=   $
��30px��6N   $	��654��6\   $��0 0 0 2q�6h   6�   54      � 56   
   $��10 5 0 0q�6   $��5��6&   .+�57      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $��Ответ 3:��6T   6f   �� 58      -   5        $ response3Text��58      $
��20px(�6-   $��0 0 0 1q�6=   $
��30px��6N   $	��654��6\   $��0 0 0 2q�6h   6�   59      � 5;   
   $��10 5 0 0q�6   $��6��6&   .+�5<      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $��Ответ 4:��6T   6f   �� 5=      -   5        $ response4Text��5=      $
��20px(�6-   $��0 0 0 1q�6=   $
��30px��6N   $	��654��6\   $��0 0 0 2q�6h   6�   5>      � 5@   
   $��10 5 0 0q�6   $��7��6&   .+�5A      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $��Ответ 5:��6T   6f   �� 5B      -	   5        $ response5Text��5B      $
��20px(�6-   $��0 0 0 1q�6=   $
��30px��6N   $	��654��6\   $��0 0 0 2q�6h   6�   5C      �� 5E   
   -
   5         ��Name$ 
mainButton��5E      $��8��6%   $	��200��62   $��30��6>   $#��Добавить вопрос��6J   6~   5F      5G      