   M S B A M L   `   `   `  ����G  CAutoUploader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:AutoUploaderAutoUploader  5   	       AutoUploader.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:AutoUploader     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � -   5         �Name$ MainGrid��5	      .+�5   
   �� -   5         ��Name$ TerminalTextBox��5       �� 
IsReadOnly$
 True��6+   " �� VerticalScrollBarVisibility$
 Auto=�6=   
���   �
�������$��Consolas)�5   8   $��12(�6N   $��Center=�6\   $
��Left=�6w   $	��587��5      $	��434��6   �� 5   
   -   5         ��Name$ Files��5      $"��Total: 0 Success: 0 Fails: 0��6   $��587,358,0,-358q�6F   $
��Left=�6^   $	��800��6y   � 5   
   $��-175,-108,0,108q�6   $
��Left=�6(   $	��779��6C   .+�5      �� -   5         ��Name$ 	UploadBtn��5      $��Upload��6(   $��897,193,-167,192q�69   �� 5      -   5        $ 
SuggestBtn��5      $��Suggest��6)   $��897,114,-167,271q�6;   6U   5      � 5   
   -   5        $ LeftGrid��5      $	��Top=�6!   $
��Left=�69   $	��150��6T   $��184,416,0,0q�6`   .+�5      �� $��410,-412,-383,175q�6   .+�5      �� $��
Headless?:��6   $��5q�6-   	� 5      -   5         	�Name$ RadioButton3��5      $	��Yes��64    	� 	GroupName$ DriverOptions��6B   $��5q�6\   	� 5      -   5        $ RadioButton4��5      $��No��64   $ DriverOptions��6A   $��5q�6[   �� 5      -	   5        $ HeadlessLabel��5      $��5q�60   $��24��6;   	 J� RenderTransformOrigin$	 0.5,0.5&�6G   $	��139��6g   z�5      V� 5      .~�5      �� 
 �� ScaleY$
 -1Q�6-   �� 5      �� 5      T� 5       5!      5#      �� 5&      $��Enable Suggestions:��6   $��5q�66   $	��117��6A   �� 5'      -
   5         ��Name$ EnableSuggestionsCheckBox��5'      $��5q�6>   $	��109��6I   ��5(      �� 5)       �� 
TargetType" ��6    .�5*      ��  �� Property � �� �� Value 5+   "   �� 5,   &    �� 
TargetType" ��66   .R�5-   *   � $��50��6/   $��25��6:   .+�5.   .   �� $��SwitchBorder��65    �� CornerRadius$
 12.5��6I   
�������6]   A� 5/   .   $��SwitchThumb��66   $��20��6I   $��20��6T   
�������$��5q�6m   �� 50   .   51   +    �� Triggers 52   *   P� 53   .    P� Property �@d�	IsChecked P� Value$
 TrueY�6K   . �54   2   ��  �� 
TargetName$ SwitchBorder��69    � ��
 � � �6i   �� 55   2   $ SwitchThumb��69    � ��$ 27.5,2.5,2.5,2.5q�6d   56   /   58   '   5:      5;      5=      5>      5?      � 5B   
   -   5        $ 	RightGrid��5B      $	��Top=�6"   $��Center=�6:   $��
150,20,0,0q�6W   .+�5C      �� $��128,250,-155,-266q�6   5D      5E      5F      5G      