   M S B A M L   `   `   `  ����K  GPeoplePaneRegion, Version=5.4.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls  5      I ECustomControls, Version=5.4.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CustomControls;assembly=CustomControlsCustomControls 5      /    )OutlookAddIn1.UserControls.PolicyTypeList   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:OutlookAddIn1.UserControls     5      Ccc4clr-namespace:CustomControls;assembly=CustomControls     5      -   5          � 	Resources  5
         ScrollBarTrackThumb �� 
TargetType �� Property �� Value �� 
TargetType �Name ��Name �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName � RowDefinitions ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton P� 
SourceName  ButtonSelectBrush  	DarkBrush �� Angle��%�	  &	        '�� �    �� 5   
   " n�5   	   .�5      ��  � �� 5      �� 5      " n�6&   .R�5      � $
 Grid��6   .+�5      �� $��Stretch=�6(   $��Stretch=�5   !   $
��Auto��5   !   $
��Auto��5   !   
������ 5   !   �� 5      $ CornerScrollBarRectangle��6%   $ 5��5      $��Stretch=�5      $��Stretch=�5      $
��Auto��5      $
��Auto��5      $��0,1,0,1q�5      #��z��5      5       5      P� 5       	 �@�Tag$
 
Horizontal��5!   !   . �5"   "   �� $ CornerScrollBarRectangle��6)    � ��$
 Auto��5$   !   �� 5%   "   $ CornerScrollBarRectangle��6)    � ��$ 6��5'   !   5(      5*      5,      5-      �� 5/   
   " ��6   .�50      ��  �@��IsFlicksEnabled .  51      �� 52       � �� �#ADABAB53      �� 54       � ��
 ���� 55      �� 56       � ��$ 7��57      �� 58       � �� 59      �� 5:      " ��6&   .R�5;      � $ GridRoot��6   $��7��5<      #��z��5=       5>      �� 5?   "   $��0.00001* �60   .+�5A      ]� $ 
PART_Track��6$   $��0��5B      ��. |�.   5E   "   n� 5F   &   $ Thumb��6,   #��z��5G   %   #���   5H   %    5J   "   �� 5K   &   $ PageUp��63   $��ScrollBar.PageDownCommand��5L   -   $ 0Q�5M   -   |�.  5N   -    5P   "   �� 5Q   &   $ PageDown��63   $��ScrollBar.PageUpCommand��5R   -   $ 0Q�5S   -   |�.  5T   -   5V      5W       5X      P� 5Y      $ Thumb��6&   	 �@J�IsMouseOver
 . . �5\   "   �� # �  6)   $ Thumb��5]   !    � ��5^   !   5_      P� 5`      $ Thumb��6&   	 �@n�
IsDragging
 . . �5c   "   �� # �  6)   $ Thumb��5d   !    � ��5e   !   5f      P� 5h      	 � {�
 .  . �5j   "   �� $ Thumb��6)    � y�$ 	Collapsed=�5l   !   5m      P� 5n      	 �@��Orientation$
 
Horizontal=�5o   !   . �5p   "   �� $ GridRoot��6)    �@�LayoutTransform 5r   &   �� 5s   *   $	 -90Q�6:   5u   #   �� 5v   "   $ 
PART_Track��6)    �@�LayoutTransform 5x   &   �� 5y   *   $	 -90Q�6:   5{   #   �� 5|   "    � ��$
 Auto��5}   !   �� 5~   "    � ��$ 8��5   !   �� 5�   "   $ Thumb��6)    �@�Tag$ 
Horizontal��5�   !   �� 5�   "   $ PageDown��6)    � ��$ ScrollBar.PageLeftCommand��5�   !   �� 5�   "   $ PageUp��6)    � ��$  ScrollBar.PageRightCommand��5�   !   5�      5�      5�      5�      5�      .��5�      � .+�5�   
   � $��2q�6   
������ $��Stretch=�64   $��Stretch=�6P     5�        HeaderGradientBrush �� EndPoint �� 
StartPoint � Color � Offset��%�   &	       �� 5�      $ 0.5,1&�6B   $ 0.5,0&�6S   .��5�      � $ 	#FF1B405D��6#   $ 1Q�65   � 5�      $ 	#FF2F7CA8��6#   $ 0Q�65   5�      5�      .+�5�      !  CustomControls.CustomButton  -   5        $��btnPolicyTypePopupClose��5�   �      BackgroundImage$ ../Asset/close_icon.png��6    K� ZIndex$
 1000��6H   $��	0,20,30,0q�6\   $��24��6o   $��24��6{   $	��Top=�6�   $��Right=�6�   6�   � 5�      
�������$��
30 20 30 0q�6)    5�      �� 5�      $��50 �6$   62   �� 5�      $��5 �6$   61   �� 5�      $��* �6$   61   �� 5�      $��36 �6$   .+�5�      � -   5        $��0��5�      $��Stretch=�6$   $��Stretch=�6k   .+�5�      �� $��Policy type list��6    $��OpenSans)�69   $��16(�6P   $��SemiBold"�6^   $��8 4 0 0q�6t   5�      �� 5�      $��24��6   $	��170��6%   $��Right=�61   $��	0,22,27,0q�6M   $��Center=�6a   ���#c6c6c6$��1q�6�   ._�5�      �  � ColumnDefinitions 5�      �� 5�      $	��11* �6/   �� 5�      $	��17* �6/   .+�5�      �� $��14��6    $��2,4,0,4q�6,   $��14��6=   $
��Left=�6H   $!��../Asset/textbox_search.png��6c   $
��Fill=�6�   $y�Visible=�6�   �� 5�      -   5        $��policyTypeSearchTextBox��5�   �   $��0q�6"   $��20��66   $	��135��6B   $��Center=�6�   $��12(�6�   $��OpenSans)�6�   $
��Left=�6�   $��17,1,0,1q�6   $��2��6  �� 5�       J� IsHitTestVisible .  $��Search��69   $��12(�6J   $��OpenSans)�6X   $��Light"�6n   $��Center=�6�   $
��Left=�6�   $��14,0,0,0q�6�   ���	#FF9d9d9d��5�      �� 5�   "   " ��6(   .�5�   &   ��  � y�$ 	Collapsed=�6C    �� Triggers 5�   &   �� 5�   *    �� Binding 66   �� *Text+  �� ElementName$  policyTypeSearchTextBox��! �� Value!  6t   .a�5�   .   ��  � y�$ Visible=�6K   5�   +   5�   #   5�      �� 5�      -   5        $��clearFiltertext��5�   s   #��[" �6!   $��16��6\   $��16��6h   $y�	Collapsed=�6�   $��0,3,2,3q�6�   $��Right=�6�   $��0q�6�   $��1��6   .��5�      �� $��10��6$   $��10��60   $"��../Asset/searchbox_clear.png��6;   $
��Fill=�6a   $y�Visible=�6p   5�      5�      5�      &   System.Windows.Controls.DataGrid  5�      -   5        "  Name$" DgPolicyType��5�   E   $y�Visible=�6   $��	10,0,10,0q�60   #   RowHeaderWidth$# 0Q�6�   
�������$��0 0 0 0q�6�   $
��Auto=�6�   $ �� IsTextSearchEnabled$ .  % �� IsTextSearchCaseSensitive% .  &   
IsReadOnly& . $��2��6s  '   AutoGenerateColumns' .  (   GridLinesVisibility$
( None=�6�  ���	#FFC8EBFC$��0q�6�  )   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$) * 6�  $��Stretch=�6�  ��6[   �� *PolicyTypeFilteredItems+  5�      *  $System.Windows.Controls.DataGridCell* �� Padding+ J� SnapsToDevicePixels, � ToolTip=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%  '        '  �    �� 5�      "  6    .�5�      ��  � ��$ Center=�6I   �� 5�       � �� 5�   "   �� 5�   &   "  66   .R�5�   *   �� $* 3q�61   #��z��6=   $��0q�6i   #��z��6}   + . ._�5�   .   �� #+ z+ 6?   $��Center=�6{   ��5�   2   �� 5�   6   .R�5�   :   �� $��	mainblock��6D   $��CharacterEllipsis=�6U   $��25��6v   $��Calibri)�6�   $��13(�6�   $��Center=�6�   $��5 5 0 0q�6�   ��6�   �� *Text+, 6�   �� *Text+5�   7   5�   /   5�   +    5�   *   P� 5�   .   	 �@ 
IsSelected
 . . �5�   2   ��  � ��
 �����6O   �� 5�   2    � �� �#8dca356O   5�   /   P� 5�   .   	 �@ 
IsSelected
 .  . �5�   2   ��  � �� �#6666666O   5�   /   5�   '   5�      5�      �� 5�      "  6    .�5�      ��  � �� 5�   "   �� 5�   &   "  66   .R�5�   *   �� #��z��60   $��Center=�6T   $��Center=�6o   $��40��6�   $��Calibri)�6�   $��13(�6�   ���#0072cd
�������6�   5�   '   5�      5�      5�      -   RowHeaderStyle- 5�      �� 5�      .�5�      ��  � �� �#c7eafc6C   �� 5�       � ��$ Center=�6J   �� 5�       � ��$ Center=�6Q   5�      .   Columns. 5�      0  *System.Windows.Controls.DataGridTextColumn  5�      /   
Foreground/ �#6666660   
FontFamily$0 Calibri)�6B   ,  &System.Windows.Controls.DataGridColumn1   Header$1 Policy Type��6W   2   Width$	2 150 6m   1  +System.Windows.Controls.DataGridBoundColumn3   Binding3 6y   �� *PolicyTypeCode+  5�      $0 Calibri)�6-   $1 Policy Description��6B   3 6^   �� *PolicyTypeDescription+5�      �� 5�      4 �� Orientation$4 
Horizontal=�6   $��4��66   $��0,0,5,0q�6C   $��Right=�6T   .+�5�        -   5        5  Name$5 btnDonePolicyType��5�   �   {�.  $��0��68   $
��Left=�6h   $��0q�6�   $ ../Asset/done@2x.png��6�   
������ $��60��6�   $��30��6   $��Search��6  $��0 0 0 0q�6  5�        5�      -   5        $5 btnCancelPolicyType��5�   s   $��0��6&   $
��Left=�6X   $��10 0 0 0q�6�   $��0q�6�   $ ../Asset/cancel@2x.png��6�   
������ $��60��6�   $��30��6  $��Search��6  6$  5�      5�      5�      5�      5�      