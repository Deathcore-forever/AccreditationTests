   M S B A M L   `   `   `  ����M  IAccreditationTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:AccreditationTests.AdminAccreditationTests.Admin  5   	   *    $AccreditationTests.Admin.AddEditTest   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:AccreditationTests.Admin     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5	   	   $	��450��5
   	   $	��800��6   $��Consolas)�6"   .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   6*   �� 5      6   �� 5      6   �� 5      $��* �6   6)   .+�5   
   ]� $��0��6   ���#73B8CD.+�5      �� -   5         ��Name$ WindowHeader��5      $��Center=�6.   $��Center=�6K   
�������$
��24px(�6y   $
��Bold"�6�   Создание теста6�   5      � 5   
   $��10 5 0 0q�6   $��1��6&   $��Center=�63   $��Center=�6N   $	��700��6k   .+�5      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $"��Название теста:��6T   6m   �� 5      -   5         ��Name$ testNameText��5      $
��20px(�6,   $��0 0 0 1q�6<   $
��30px��6M   $	��530��6[   $��0 0 0 2q�6g   5      � 5   
   $��10 5 0 0q�6   $��2��6&   $��Center=�64   $��Center=�6O   $	��700��6l   .+�5      �� ���#317F97$
��20px(�6-   $��	ExtraBold"�6=   $&�� Вариантов ответа:��6T   6o   �� 5      -   5         ��Name$ responseNumberText��5   D   
�������$��0 0 0 2q�6*   $��250px��6`   $��10 0 0 0q�6n   .@�5      �� $��4��6   �� 5       $��5��6   5!      5"      �� 5$   
   -   5         ��Name$ addTestButton��5$      $��7��6(   $	��200��65   $��30��6A   6h   5%      5&      