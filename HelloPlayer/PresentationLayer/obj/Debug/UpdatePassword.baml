   M S B A M L   `   `   `  ����L  HPresentationLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:PresentationPresentation  5   	   !    Presentation.UpdatePassword   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Presentation     5   	   ���#0b409c  � 
ResizeMode$  NoResize=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � Title$ UpdatePassword��5   	   $	��300��6    $	��450��6-   .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      $	��225 �6   6-   �� 5      6!   .+�5   
   �� -   5         ��Name$ lblCurrentPassword��5      $��0��5      $��1��6    $��Center=�5      $��Right=�5      $��Current Password:��5      $��Center=�5      $��Right=�5      $
��10 0q�5       ���#fdbe34$��DemiBold"�5"      �� 5#   
   -   5        $ lblNewPassword��5#      $��0��5$      $��2��6    $��Center=�5%      $��Right=�5&      $��New Password:��5'      $��Center=�5(      $��Right=�5)      $
��10 0q�5*      ���#fdbe34$��DemiBold"�5,      �� 5-   
   -   5        $ lblRetypePassword��5-      $��0��5.      $��3��6    $��Center=�5/      $��Right=�50      $��Retype Password:��51      $��Center=�52      $��Right=�53      $
��10 0q�54      ���#fdbe34$��DemiBold"�56      �� 57   
   -   5         ��Name$ 	btnSubmit��57      $��0��58      $��4��6!   $��Submit��59      $��Right=�5:      $��5q�5;      $
��10 5q�6   $��DemiBold"�5<      6'   G� 5=   
   -   5         G�Name$ pwdCurrentPassword��5=      $��1��5>      $��1��6&   $	��120��5?      $��30��6"   $��5q�5@      $��Center=�5A      $��Center=�5B      $
��Left=�5C      G� 5D   
   -   5        $ pwdNewPassword��5D      $��1��5E      $��2��6&   $	��120��5F      $��30��6"   $��5q�5G      $��Center=�5H      $��Center=�5I      $
��Left=�5J      G� 5K   
   -   5        $ pwdRetypePassword��5K      $��1��5L      $��3��6&   $	��120��5M      $��30��6"   $��5q�5N      $��Center=�5O      $��Center=�5P      $
��Left=�5Q      5R      5S      