   M S B A M L   `   `   `  ���� K  GPeoplePaneRegion, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls  I ECustomControls, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CustomControls;assembly=CustomControlsCustomControls /    )OutlookAddIn1.UserControls.PolicyTypeList   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:OutlookAddIn1.UserControls     Ccc4clr-namespace:CustomControls;assembly=CustomControls     -     � 	Resources     ScrollBarTrackThumb �� 
TargetType �� Property �� Value �� 
TargetType �Name ��Name �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName � RowDefinitions ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton P� 
SourceName  ButtonSelectBrush  	DarkBrush �� Angle��%�  &	        '�� �    �� " n�.���  � �� �� " n�.R�� $
 Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��
������ �� $ CornerScrollBarRectangle��$ 5��$��Stretch=�$��Stretch=�$
��Auto��$
��Auto��$��0,1,0,1q�#��z�� P� 	 �@�Tag$
 
Horizontal��. ��� $ CornerScrollBarRectangle�� � ��$
 Auto���� $ CornerScrollBarRectangle�� � ��$ 6���� " ��.���  �@��IsFlicksEnabled .  ��  � �� �#ADABAB��  � ��
 ���� ��  � ��$ 7����  � �� �� " ��.R�� $ GridRoot��$��7��#��z�� �� $��0.00001* �.+�]� $ 
PART_Track��$��0����. |�.   n� $ Thumb��#��z��#���    �� $ PageUp��$��ScrollBar.PageDownCommand��$ 0Q�|�.   �� $ PageDown��$��ScrollBar.PageUpCommand��$ 0Q�|�.   P� $ Thumb��	 �@J�IsMouseOver
 . . ��� # �  $ Thumb�� � ��P� $ Thumb��	 �@n�
IsDragging
 . . ��� # �  $ Thumb�� � ��P� 	 � {�
 .  . ��� $ Thumb�� � y�$ 	Collapsed=�P� 	 �@��Orientation$
 
Horizontal=�. ��� $ GridRoot�� �@�LayoutTransform �� $	 -90Q��� $ 
PART_Track�� �@�LayoutTransform �� $	 -90Q���  � ��$
 Auto����  � ��$ 8���� $ Thumb�� �@�Tag$ 
Horizontal���� $ PageDown�� � ��$ ScrollBar.PageLeftCommand���� $ PageUp�� � ��$  ScrollBar.PageRightCommand��.��� .+�� $��2q�
������ $��Stretch=�$��Stretch=�    HeaderGradientBrush �� EndPoint �� 
StartPoint � Color � Offset��%d   &	       �� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF1B405D��$ 1Q�� $ 	#FF2F7CA8��$ 0Q�.+�!  CustomControls.CustomButton  -   $��btnPolicyTypePopupClose��   BackgroundImage$ ../Asset/close_icon.png�� K� ZIndex$
 1000��$��	0,20,30,0q�$��24��$��24��$	��Top=�$��Right=�� 
�������$��
30 20 30 0q� �� $��50 ��� $��5 ��� $��* ��� $��36 �.+�� -   $��0��$��Stretch=�$��Stretch=�.+��� $��Policy type list��$��OpenSans)�$��16(�$��SemiBold"�$��8 4 0 0q��� $��24��$	��170��$��Right=�$��	0,22,27,0q�$��Center=����#c6c6c6$��1q�._��  � ColumnDefinitions �� $	��11* ��� $	��17* �.+��� $��14��$��2,4,0,4q�$��14��$
��Left=�$!��../Asset/textbox_search.png��$
��Fill=�$y�Visible=��� -   $��policyTypeSearchTextBox��$��0q�$��20��$	��135��$��Center=�$��12(�$��OpenSans)�$
��Left=�$��17,1,0,1q�$��2����  J� IsHitTestVisible .  $��Search��$��12(�$��OpenSans)�$��Light"�$��Center=�$
��Left=�$��14,0,0,0q����	#FF9d9d9d���� " ��.���  � y�$ 	Collapsed=� �� Triggers ��  �� Binding �� *Text+  �� ElementName$  policyTypeSearchTextBox��! �� Value!  .a���  � y�$ Visible=��� -   $��clearFiltertext��#��[" �$��16��$��16��$y�	Collapsed=�$��0,3,2,3q�$��Right=�$��0q�$��1��.���� $��10��$��10��$"��../Asset/searchbox_clear.png��$
��Fill=�$y�Visible=�&   System.Windows.Controls.DataGrid  -   "  Name$" DgPolicyType��$y�Visible=�$��	10,0,10,0q�#   RowHeaderWidth$# 0Q�
�������$��0 0 0 0q�$
��Auto=�$ �� IsTextSearchEnabled$ .  % �� IsTextSearchCaseSensitive% .  &   
IsReadOnly& . $��2��'   AutoGenerateColumns' .  (   GridLinesVisibility$
( None=����	#FFC8EBFC$��0q�)   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$) * $��Stretch=����� *PolicyTypeFilteredItems+  *  $System.Windows.Controls.DataGridCell* �� Padding+ J� SnapsToDevicePixels, � ToolTip=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%Z  '        '  �    �� "  .���  � ��$ Center=���  � �� �� "  .R��� $* 3q�#��z��$��0q�#��z��+ . ._��� #+ z+ $��Center=����� .R��� $��	mainblock��$��CharacterEllipsis=�$��25��$��Calibri)�$��13(�$��Center=�$��5 5 0 0q����� *Text+, �� *Text+ P� 	 �@ 
IsSelected
 . . ���  � ��
 �������  � �� �#8dca35P� 	 �@ 
IsSelected
 .  . ���  � �� �#666666�� "  .���  � �� �� "  .R��� #��z��$��Center=�$��Center=�$��40��$��Calibri)�$��13(����#0072cd
�������-   RowHeaderStyle- �� .���  � �� �#c7eafc��  � ��$ Center=���  � ��$ Center=�.   Columns. 0  *System.Windows.Controls.DataGridTextColumn  /   
Foreground/ �#6666660   
FontFamily$0 Calibri)�,  &System.Windows.Controls.DataGridColumn1   Header$1 Policy Type��2   Width$	2 150 1  +System.Windows.Controls.DataGridBoundColumn3   Binding3 �� *PolicyTypeCode+  $0 Calibri)�$1 Policy Description��3 �� *PolicyTypeDescription+�� 4 �� Orientation$4 
Horizontal=�$��4��$��0,0,5,0q�$��Right=�.+�  -   5  Name$5 btnDonePolicyType��{�.  $��0��$
��Left=�$��0q�$ ../Asset/done@2x.png��
������ $��60��$��30��$��Search��$��0 0 0 0q�  -   $5 btnCancelPolicyType��$��0��$
��Left=�$��10 0 0 0q�$��0q�$ ../Asset/cancel@2x.png��
������ $��60��$��30��$��Search��