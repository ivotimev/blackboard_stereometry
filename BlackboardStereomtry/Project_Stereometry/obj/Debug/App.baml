   M S B A M L   `   `   `  ����N  JProject_Stereometry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Project_StereometryProject_Stereometry  5          Project_Stereometry.App   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      3local!clr-namespace:Project_Stereometry     5        �� 	Resources  5       �� 
TargetType   
MainCanvas �� Property �� Value<   6Project_Stereometry.BackgroundToGradientColorConverter  gradientConverter  ExpanderHeaderFocusVisual J� SnapsToDevicePixels �� StrokeDashArray  ExpanderDownHeaderStyle �� 
TargetType �Name J� RenderTransformOrigin	 A�Name
 �� 	Converter J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   Opacity   
BlurRadius   ShadowDepth   	Direction �� ScaleX �� ScaleY ��Name J� Opacity �� Triggers P� Property P� Value �� 
TargetName  BB_Menu_Expander �� CornerRadius ��Name �� Orientation d�Name � FocusVisualStyle d� 	IsChecked �� Mode �� RelativeSource  ��Name  ExpanderRightHeaderStyle  BB_ExpanderDownHeaderStyle  BB_Menu_Expander_Prime%   Project_Stereometry.BB_Expander!   ElipseRadius" ^� InitialShowDelay# � ToolTip$   TooltipIconSource%   BB_TooltipTitle&   BB_TooltipContent' �� TextAlignment  SmallRemoteToggleStyle 	 FocusVisual 
 Button.Static.Background( �� Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border  Button.Disabled.Foreground  
MenuButton) ��Name* ��Name ! RadioButton.Static.Background  RadioButton.Static.Border  OptionMarkFocusVisual $  RadioButton.MouseOver.Background   RadioButton.MouseOver.Border  RadioButton.MouseOver.Glyph # RadioButton.Disabled.Background  RadioButton.Disabled.Border  RadioButton.Disabled.Glyph " RadioButton.Pressed.Background  RadioButton.Pressed.Border  RadioButton.Pressed.Glyph   RadioButton.Static.Glyph ! InstrumentRadioButton��%�6  &	        &	 J     &	 ]     &	      7  &	 v    7  7  7  &	 �    7  &	 (    7  7  &	     7  7  &		 �#    &	
 E$    &	 f$    &	 �$    &	 �$    &	 �$    &	 �$    &	 %    &	 6%    &	 Y%    &	 |%    7 	 7 
 &	 �+    &	 ,    &	 &,    &	 �,    &	 �,    &	 -    &	 @-    &	 c-    &	 �-    &	 �-    &	 �-    &	 �-    &	  .    &	! 5.    7 	 7  7  �� 5   
   " ��6   .�5	      ��  � ��
 ���� 6+   5
        5   
   63   �� 5?   
   .�5@      ��  � �� 5A      �� 5B      .R�5C      �� ._�5D      �� $��0q�6(    . 
���   �$��1��6]   $	 1 2R�6q   5E      5F      5H      5I      �� 5J   
   " d�60   .�5K      ��  � �� 5L      �� 5M      " d�6&   .R�5N      �� ._�5O      � $ 
elipseGrid��6~   
������ #��z��6<   #��z��6\   $ 0.5, 0.5&�6�   .+�5P   "   A� $	 gradientElipse��6*   $��Stretch=�6�   $��Stretch=�6�   ��6B   �� *
Background+8
    5Q   &     5R   *   $
 0.45Q�6;   $ 2Q�6J   $ 3Q�6Y   $	 300Q�6i   5T   #   �� 5U   "   $��Center=�63   $��Center=�6P   $ 0.5,0.5&�6k   $��2,2,0,0q�6�   z�5V   &   �� 5W   *   $
 0.82Q�69   $
 0.82Q�6G   5Y   #   �� 5Z   "   $ clicked��6(   $'��!Custom_Toggle/BB_Highlighter1.png��69   $ 0Q�6d   $��Stretch=�6p   $��Stretch=�6�   $ 0.5,0.5&�6�   z�5[   &   �� 5\   *   $
 0.88Q�69   $
 0.88Q�6G   5^   #   �� 5_   "   $ 	highlight��6(   $&�� Custom_Toggle/BB_Highlighter.png��6;   $ 0Q�6e   $��Stretch=�6q   $��Stretch=�6�   �� 5`   "   $ pressed��6(   $"��Custom_Toggle/BB_Pressed.png��69   $ 0Q�6_   $��Stretch=�6k   $��Stretch=�6�   �� 5a   "   $ disabled��6(   $+��%Custom_Toggle/BB_Disabled_Overlay.png��6:   $ 0Q�6i   $��Stretch=�6u   $��Stretch=�6�   5b      5c       5d      P� 5e       �@d�	IsChecked$
 trueY�6;   . �5f   "   �� $ clicked��6I    �@J�Opacity$
 0.85Q�6<   6I   5g      P� 5h       �@J�IsMouseOver . . �5i   "   �� $ 	highlight��6I    �@J�Opacity$
 0.75Q�6<   6I   5j      P� 5k       � �� . . �5l   "   �� $ pressed��6F    �@J�Opacity$ 1Q�6<   6F   �� 5m   "   $ 	highlight��6H    �@J�Opacity$	 0.2Q�6<   6H   �� 5n   "   $ 
elipseGrid��6D    � z� 5o   &   �� 5p   *   $
 0.95Q�69   $
 0.95Q�6G   5r   #   5s      P� 5t       � {� .  . �5u   "   �� $ disabled��6F    �@J�Opacity$ 1Q�6<   6F   5v      5x      5z      5{      �� 5|   
   " 9�6)   .�5}      ��  � ��# � ��6=   �� 5~       � ��
 ���� 6+   �� 5       � ��$ Stretch=�6;   �� 5�       � ��$ Stretch=�69   �� 5�       � ��
 ���� 6,   �� 5�       � ��$ 0q�60   �� 5�       � �� 5�      �� 5�      " 9�6&   .R�5�      �� #��z��6!   #��z��6M   $ 3��6�   ._�5�      �� $ Stack��6@   $ Vertical=�6)   .+�5�   "   d� $ 
HeaderSite��6/   #��z��6C   8   #��z��6�   $��0��6M  $��0��6Z  $��65��6h  $��65��6s  8�� #��z��6�  #��z��6�   6�   �� *
IsExpanded+$ TwoWay=� �� *TemplatedParent+6�  �� 5�   "   $  
ExpandSite��63   |�.  $
��Left=�6Y   #��z��6t   $y�	Collapsed=�6�   $��Center=�6�   5�      5�       5�      P� 5�       �@9�
IsExpanded . . �5�   "   �� $ 
ExpandSite��6?    � y�$ Visible=�6W   5�      P� 5�       �@9�ExpandDirection$ Right=�6A   . �5�   "   �� $ Stack��6:    � �� 5�   &   �� 5�   *   " ��60   .�5�   .   ��  �@��Orientation$ 
Horizontal=�6L   5�   +   5�   #   �� 5�   "   $ 
ExpandSite��6H    � ��$
 Left=�6`   �� 5�   "   $ 
HeaderSite��6:   8   � ��6R   5�      P� 5�       � {� .  . �5�   "   �� # � ��6?    � ��5�      5�      5�      5�      �� 5�   
   " d�63   .�5�      ��  � �� 5�      �� 5�      " d�6&   .R�5�      �� $
��Left=�6!   ._�5�      � $ 
elipseGrid��6~   
������ #��z��6<   #��z��6\   $ 0.5, 0.5&�6�   $
��Left=�6�   .+�5�   "   A� $	 gradientElipse��6*   $��Stretch=�6�   $��Stretch=�6�   ��6B   �� *
Background+8
    5�   &     5�   *   $
 0.45Q�6;   $ 2Q�6J   $ 3Q�6Y   $	 300Q�6i   5�   #   �� 5�   "   $��Center=�63   $��Center=�6P   $ 0.5,0.5&�6k   $��2,2,0,0q�6�   z�5�   &   �� 5�   *   $
 0.82Q�69   $
 0.82Q�6G   5�   #   �� 5�   "   $ clicked��6(   $'��!Custom_Toggle/BB_Highlighter1.png��69   $ 0Q�6d   $��Stretch=�6p   $��Stretch=�6�   $ 0.5,0.5&�6�   z�5�   &   �� 5�   *   $
 0.88Q�69   $
 0.88Q�6G   5�   #   �� 5�   "   $ 	highlight��6(   $&�� Custom_Toggle/BB_Highlighter.png��6;   $ 0Q�6e   $��Stretch=�6q   $��Stretch=�6�   �� 5�   "   $ pressed��6(   $"��Custom_Toggle/BB_Pressed.png��69   $ 0Q�6_   $��Stretch=�6k   $��Stretch=�6�   �� 5�   "   $ disabled��6(   $+��%Custom_Toggle/BB_Disabled_Overlay.png��6:   $ 0Q�6i   $��Stretch=�6u   $��Stretch=�6�   5�      5�       5�      P� 5�       �@d�	IsChecked$
 trueY�6;   . �5�   "   �� $ clicked��6I    �@J�Opacity$
 0.85Q�6<   6I   5�      P� 5�       �@J�IsMouseOver . . �5�   "   �� $ 	highlight��6I    �@J�Opacity$
 0.75Q�6<   6I   5�      P� 5�       � �� . . �5�   "   �� $ pressed��6F    �@J�Opacity$ 1Q�6<   6F   �� 5�   "   $ 	highlight��6H    �@J�Opacity$	 0.2Q�6<   6H   �� 5�   "   $ 
elipseGrid��6D    � z� 5�   &   �� 5�   *   $
 0.95Q�69   $
 0.95Q�6G   5�   #   5�      P� 5�       � {� .  . �5�   "   �� $ disabled��6F    �@J�Opacity$ 1Q�6<   6F   5�      5�      5�      5�      �� 5�   
   "  6/   .�5�      ��  � ��# � ��6=   �� 5�       � ��
 ���� 6+   �� 5�       � ��$ Stretch=�6;   �� 5�       � ��$ Stretch=�69   �� 5�       � ��
 ���� 6,   �� 5�       � ��$ 0q�60   �� 5�       �@ ElipseRadius$ 65Q�6-   �� 5�       � �� 5�      �� 5�      "  6&   .R�5�      �� #��z��6!   #��z��6M   $ 3��6�   ._�5�      ]� .+�5�   "   d� $ 
HeaderSite��6/   #��z��6C   $	��Top=�6f   8   #��z��6�   $��0��6b  $��0��6o  #��z! 6}  #��z! 6�  8�� #��z��6   #��z��6F  $
" 1100��6p  #��z��6�   6  �� *
IsExpanded+$ TwoWay=� �� *TemplatedParent+# 5�   &   ]� 5�   *   .+�5�   .   �� 
�������$��0,0,0,1q�6I   $	��Top=�6c   $��0,0,0,0q�6x   ._�5�   2   �� $ 
Horizontal=�6=   .+�5�   6   �� $��21��6<   $��21��6H   #��z$ 6S   $��Center=�6�   �� 5�   6   $��13(�6@   $
��Bold"�6N   $	��Top=�6`   $��21��6u   #��z% 6�   $��Center=�6�   $��Center=�6�   5�   3   5�   /   �� 5�   .   $��11(�68   $��1,3,1,0q�6F   #��z& 6W   $	��115��6�   $��WrapWithOverflow=�6�   $' Right=�6�   5�   +   5�   #   �� 5�   "   $  
ExpandSite��63   $��Bottom=�6G   |�.  $
��Left=�6q   #��z��6�   $y�	Collapsed=�6�   $��Center=�6�   5�      5�       5�      P� 5�       �@9�
IsExpanded . . �5�   "   �� $ 
ExpandSite��6?    � y�$ Visible=�6W   5�      P� 5�       �@9�ExpandDirection$ Right=�6A   . �5�   "   �� $ 
ExpandSite��6C    � ��$ Right=�6[   �� 5�   "   $ 
HeaderSite��6C    � ��$
 Left=�6[   5     P� 5      � {� .  . �5  "   �� # � ��6?    � ��5     5     5     5	     �� 5  
   " d�6/   .�5     ��  �@�FocusVisualStyle8   61   �� 5      � �� 5     �� 5     " d�6&   .R�5     �� ._�5     � $ 
elipseGrid��6~   
������ #��z��6<   #��z��6\   $ 0.5, 0.5&�6�   .+�5  "   A� $	 gradientElipse��6*   $��Stretch=�6�   $��Stretch=�6�   ��6B   �� *
Background+8
   5  &     5  *   $
 0.45Q�6;   $ 2Q�6J   $ 3Q�6Y   $	 300Q�6i   5  #   �� 5  "   $��Center=�63   $��Center=�6P   $ 0.5,0.5&�6k   $��2,2,0,0q�6�   $
 0.92Q�6�   z�5  &   �� 5  *   $
 0.62Q�69   $
 0.62Q�6G   5  #   �� 5  "   $ clicked��6(   $'��!Custom_Toggle/BB_Highlighter1.png��69   $ 0Q�6d   $��Stretch=�6p   $��Stretch=�6�   $ 0.5,0.5&�6�   z�5  &   �� 5  *   $
 0.88Q�69   $
 0.88Q�6G   5   #   �� 5!  "   $ 	highlight��6(   $&�� Custom_Toggle/BB_Highlighter.png��6;   $ 0Q�6e   $��Stretch=�6q   $��Stretch=�6�   �� 5"  "   $ pressed��6(   $"��Custom_Toggle/BB_Pressed.png��69   $ 0Q�6_   $��Stretch=�6k   $��Stretch=�6�   �� 5#  "   $ disabled��6(   $+��%Custom_Toggle/BB_Disabled_Overlay.png��6:   $ 0Q�6i   $��Stretch=�6u   $��Stretch=�6�   5$     5%      5&     P� 5'      �@d�	IsChecked$
 trueY�6;   . �5(  "   �� $ clicked��6I    �@J�Opacity$
 0.85Q�6<   6I   5)     P� 5*      �@J�IsMouseOver . . �5+  "   �� $ 	highlight��6I    �@J�Opacity$
 0.75Q�6<   6I   5,     P� 5-      � �� . . �5.  "   �� $ pressed��6F    �@J�Opacity$ 1Q�6<   6F   �� 5/  "   $ 	highlight��6H    �@J�Opacity$	 0.2Q�6<   6H   �� 50  "   $ 
elipseGrid��6D    � z� 51  &   �� 52  *   $
 0.95Q�69   $
 0.95Q�6G   54  #   55     P� 56      � {� .  . �57  "   �� $ disabled��6F    �@J�Opacity$ 1Q�6<   6F   58     5:     5<     5=     �� 5>  
   .�5?     ��  � �� 5@     �� 5A     .R�5B     �� $��2q�6$    . #��� ��6J   $��1��6�   $	 1 2R�6�   5C     5E     5F     �� 5H  
   $( #204475��6;   �� 5I  
   $( #f6f6f4��67   �� 5J  
   $( 	#FFBEE6FD��6>   �� 5K  
   $( 	#FF3C7FB1��6:   �� 5L  
   $( 	#FFC4E5F6��6<   �� 5M  
   $( 	#FF2C628B��68   �� 5N  
   $( 	#FFF4F4F4��6=   �� 5O  
   $( 	#FFADB2B5��69   �� 5P  
   $( 	#FF838383��6=   �� 5R  
   " ��6#   .�5S     ��  �@�FocusVisualStyle8   61   �� 5T      � ��8  6+   �� 5U      � �� �#192e706,   �� 5V      � �� �#f6f6f46+   �� 5W      � ��$ 2q�60   �� 5X      � ��$ Center=�6;   �� 5Y      � ��$ Center=�69   �� 5Z      � ��$ 1q�6(   �� 5[      � �� 5\     �� 5]     " ��6&   .R�5^     � $ 
buttonGrid��6   $ 0.5,0.5&�63   .+�5_     �� 
���   � 5`  "     5a  &   $
 0.45Q�67   $ 2Q�6F   $ 3Q�6U   $	 300Q�6e   5c     �� 5d     $) border��6%   #��z��65   #��z��6a   #��z��6�    . ._�5e  "   �� $  contentPresenter��63   |�.  #��z��6_   #��z��6�   ��. # z 6�   #��z��6  5f     �� 5g     $* 	highlight��6(   ���#6f9dd8���#b2e7f8$��2��6\   $	 0.2Q�6p   $y�	Collapsed=�6~   6�   �� 5h     $* disabled��6(   ���#8A8A8A$ 0Q�6I   5i      5j     P� 5k      �@��IsDefaulted . . �5l  "   �� $ border��6@   # � ��6T    � ��5m     P� 5n      �@J�IsMouseOver . . �5o  "   �� $ 	highlight��6?    � y�$ Visible=�6V   �� 5p  "    �@�Cursor$
 Hand��6;   5q     P� 5r      � �� . . �5s  "   �� $ 	highlight��6<    �@J�Opacity$	 0.4Q�6S   �� 5t  "   $ 
buttonGrid��6D    � z� 5u  &   �� 5v  *   $
 0.95Q�69   $
 0.95Q�6G   5x  #   �� 5y  "    �@�Cursor$
 Hand��6;   5z     P� 5{      � {� .  . �5|  "   �� $ disabled��6)    �@J�Opacity$	 0.3Q�6R   5}     5     5�     5�     �� 5�  
   $( 	#FFFFFFFF��6@   �� 5�  
   $( 	#FF707070��6<   �� 5�  
   .�5�     ��  � �� 5�     �� 5�     .R�5�     �� $��14,0,0,0q�6$    . #��� ��6Q   $��1��6�   $	 1 2R�6�   5�     5�     5�     �� 5�  
   $( 	#FFF3F9FF��6C   �� 5�  
   $( 	#FF5593FF��6?   �� 5�  
   $( 	#FF212121��6>   �� 5�  
   $( 	#FFE6E6E6��6B   �� 5�  
   $( 	#FFBCBCBC��6>   �� 5�  
   $( 	#FF707070��6=   �� 5�  
   $( 	#FFD9ECFF��6A   �� 5�  
   $( 	#FF3C77DD��6=   �� 5�  
   $( 	#FF212121��6<   �� 5�  
   $( 	#FF212121��6;   �� 5�  
   " 	�6.   .�5�     ��  �@�FocusVisualStyle8   61   �� 5�      � ��8  6+   �� 5�      � ��8  6,   �� 5�      � ��# � ��6+   �� 5�      � ��$ 1q�60   �� 5�      � �� 5�     �� 5�     " 	�6&   .R�5�     � $ templateRoot��6   
������  . $ 0.5,0.5&�6i   $��0,0,0,3q�6�   .+�5�     �� $
* rect��6(   
���   � 5�  "     5�  &   $
 0.45Q�67   $ 2Q�6F   $ 3Q�6U   $	 300Q�6e   5�     �� 5�     $) radioContentBorder��6%   #��z��6A   #��z��6m   #��z��6�   $��1��6�   ._�5�  "   �� $  contentPresenter��63   |�.  #��z��6`   #��z��6�   ��. # z 6�   #��z��6  5�     �� 5�     $* 	highlight��6(   ���#6f9dd8���#b2e7f8$��2��6\   $	 0.2Q�6p   $y�	Collapsed=�6~   6�   �� 5�     $* checked��6(   
������ #��z��6L   $��1��6r   $y�	Collapsed=�6�   $ 0.5,0.5&�6�   6�   �� 5�     $* disabled��6(   ���#8A8A8A$ 0Q�6I   5�      5�     P� 5�      �@J�IsMouseOver . . �5�  "   �� $ 	highlight��6?    � y�$ Visible=�6V   5�     P� 5�      � {� .  . �5�  "   �� $ disabled��6)    �@J�Opacity$	 0.3Q�6R   5�     P� 5�      � �� . . �5�  "   �� $ 	highlight��6<    �@J�Opacity$	 0.4Q�6S   �� 5�  "   $ templateRoot��6D    � z� 5�  &   �� 5�  *   $
 0.95Q�69   $
 0.95Q�6G   5�  #   5�     P� 5�      �@d�	IsChecked$
 trueY�6;   . �5�  "   �� $ checked��6?    � y�$ Visible=�6T   6f   �� 5�  "   $ checked��6D    � z� 5�  &   �� 5�  *   $
 1.02Q�69   $
 1.06Q�6G   5�  #   5�     P� 5�      6;   Z�  �@d�	IsChecked5�     5�     5�     5�     5�     5�     