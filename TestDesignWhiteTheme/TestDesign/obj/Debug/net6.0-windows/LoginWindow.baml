   M S B A M L   `   `   `  ����M  IAccreditationTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TestDesign
TestDesign  5   	       TestDesign.LoginWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TestDesign     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5	   	    � Title$&  Вход в приложение��5
   	   $	��450��6#   $	��800��60   $��Consolas)�6<    � Icon$ resources/appicon.png��6R   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $
��70px �6   .+�5   
   �� $��0��6   $��Center=�6"   $��Center=�6=   .+�5      � $��Center=�6    � Orientation$ Vertical=�65   $��0 0 0 30q�6L   .+�5      �� ���#317F97$
��26px(�61   $��	ExtraBold"�6A   $��
Логин��6X   6g   �� 5      -   5         ��Name$ 	loginText��5   4   $��0 0 0 2q�6   $
��26px(�6G   $
��30px��6W   $	��350��6e   5      � 5      $��Center=�6   $ Vertical=�65   $��0 0 0 10q�6L   .+�5      �� ���#317F97$
��26px(�61   $��	ExtraBold"�6A   $��Пароль��6X   6h   G� 5      -   5         G�Name$ passwordText��5   8   $��0 0 0 2q�6   $
��26px(�6N   $
��30px��6^   $	��350��6l   5      �� 5       �� TextAlignment$ Center=�6   $��350px��6/   $
��Wrap=�6=   $
��20px(�6Q   dbДля входа как студент, вводить логин и пароль не нужно6�   5      � 5   
   $��1��6   	 � ColumnDefinitions	 5      �� 5      �� 5      .+�5"      �� -   5        
 � Cursor$

 Hand��5"      $��0��6#   $��20(�6J   $
��Bold"�6X   
�������$	��300��6}   $��50��6�   ���#317F97
���OO/�$��3q�6�   .��5#      �� -   5         ��Name$ studentButText��5#      " Войти как студент6G   5$      �� 5%      -   5        $

 Hand��5%      $��1��6#   $��20(�6J   $
��Bold"�6X   
�������$	��300��6}   $��50��6�   ���#317F97
���OO/�$��3q�6�   .��5&      �� -   5        $ adminButText��5&      .,Войти как администратор6K   5'      5(      5*      5+      