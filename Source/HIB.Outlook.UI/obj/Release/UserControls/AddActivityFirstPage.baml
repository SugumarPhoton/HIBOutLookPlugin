   M S B A M L   `   `   `  ���� K  GPeoplePaneRegion, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls  a ]PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35gJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.AeroMicrosoft.Windows.Themes I ECustomControls, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CustomControls;assembly=CustomControlsCustomControls 5    /OutlookAddIn1.UserControls.AddActivityFirstPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:OutlookAddIn1.UserControls     \themeJclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero     Ccc4clr-namespace:CustomControls;assembly=CustomControls     -   $
��auto��  � 	Resources     EmptyCheckBoxFocusVisual �� Property �� Value �� StrokeDashArray J� SnapsToDevicePixels  CheckRadioFocusVisual  CheckBoxFillNormal �� Color  CheckBoxStroke �� 
TargetType �� 
TargetType �� 
Background	 �� Bullet+  %Microsoft.Windows.Themes.BulletChrome
   
Background   BorderBrush J� IsMouseOver   RenderMouseOver   RenderPressed d� 	IsChecked   	IsChecked �� Triggers P� Property P� Value  ScrollBarTrackThumb �Name ��Name �� CornerRadius �� 
TargetName � RowDefinitions ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton  P� 
SourceName  ButtonSelectBrush  	DarkBrush! �� Angle  ComboBoxNormalBorderBrush ! ComboBoxNormalBackgroundBrush #	 ComboBoxDisabledForegroundBrush #
 ComboBoxDisabledBackgroundBrush  ComboBoxDisabledBorderBrush  lblUnboldStyle   ComboBoxToggleButtonTemplate" � ColumnDefinitions# �� 	ClickMode$ �� Path% �� RelativeSource& �� Mode' J� IsHitTestVisible( �� SelectionBoxItem) �� SelectionBoxItemTemplate* �� 
IsReadOnly+ �� 
IsReadOnly, � AllowsTransparency- �� IsDropDownOpen. � ActualWidth/ �� MaxDropDownHeight-  'System.Windows.Input.KeyboardNavigation0   DirectionalNavigation��%�  &	        &	 R     &	 �     &	 �     '�� �     7  7  7   7  &	 h    '�� �    &	 �    &	     &		 !    &	
 1    &	 A    &	 Q    &	 �    7  7  7  7 
 7 
 7  7 	 '�� �    7  7  7 	 �� .���  � �� �� .R��� $��1q�$��1��
���   �$	 1 2R� . �� .���  � �� �� .R��� $��14,0,0,0q�$��1��
���   �$	 1 2R� . �� $ #F4F4F4���� $ #8E8F8F���� " ��.���  � �� �	#FFF3FFFF��  � ��8   ��  � ��8  ��  � ��$ 1q���  �@�FocusVisualStyle8  ��  � �� �� " ��.R��� 
 ����  . 	   #
 z��# z��# z # z��# z ._��� #��z��#��z��#��z����. # z  P�  � �� . . ��� 8   �@�FocusVisualStyle��  � ��$ 4,0,0,0q�P�  � {� .  . ��� # � �� � ���� " n�.���  � �� �� " n�.R�� $
 Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��
������ �� $ CornerScrollBarRectangle��$ 5��$��Stretch=�$��Stretch=�$
��Auto��$
��Auto��$��0,1,0,1q�#��z�� P�  �@�Tag$ 
Horizontal��. ��� $ CornerScrollBarRectangle�� � ��$
 Auto���� $ CornerScrollBarRectangle�� � ��$ 6���� " ��.���  �@��IsFlicksEnabled .  ��  � �� �#ADABAB��  � ��
 ���� ��  � ��$ 7����  � �� �� " ��.R�� $ GridRoot��$��7��#��z�� �� $��0.00001* �.+�]� $ 
PART_Track��$��0����. |�.   n� $ Thumb��#��z��#���   �� $ PageUp��$��ScrollBar.PageDownCommand��$ 0Q�|�.   �� $ PageDown��$��ScrollBar.PageUpCommand��$ 0Q�|�.   P� $  Thumb�� �@J�IsMouseOver . . ��� # �  $ Thumb�� � ��P� $  Thumb�� �@n�
IsDragging . . ��� # �  $ Thumb�� � ��P�  � {� .  . ��� $ Thumb�� � y�$ 	Collapsed=�P�  �@��Orientation$ 
Horizontal=�. ��� $ GridRoot�� �@�LayoutTransform �� $	! -90Q��� $ 
PART_Track�� �@�LayoutTransform �� $	! -90Q���  � ��$
 Auto����  � ��$ 8���� $ Thumb�� �@�Tag$ 
Horizontal���� $ PageDown�� � ��$ ScrollBar.PageLeftCommand���� $ PageUp�� � ��$  ScrollBar.PageRightCommand���� $ #c6c6c6���� $
 #fff���� $
 #888���� $
 #eee���� $
 #888���� " ��.���  � ��$ OpenSans)���  � ��$ 12(���  � ��$ 25����  � ��$ Center=���  � ��$ Normal"��� " d�.R�� " �� �� $��20 �.+��� $��Border��$��2��8��  $ 0��$��
1, 1, 1, 1q�8�� �� $��ButtonBorder��$��1��$��
1, 1, 1, 1q����#444$ 
0, 0, 0, 0��$��
0, 0, 0, 0q�8�� F� $��Arrow��$��1��=����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	$��Center=����#444$��Center=� P�  �@J�IsMouseOver . . ��� $ ButtonBorder�� � ��
 �����P�  �@d�	IsChecked$
 TrueY�. ��� $ ButtonBorder�� � ��
 ������� $ Arrow�� � �� �	#FF8D979EP�  � {� .  . ��� $ Border��8   � ���� $ ButtonBorder��8   � ���� $ ButtonBorder��8   � ���� 8   � ���� $ Arrow�� � �� �#999�� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  � ��
 �   ���  �@�FocusVisualStyle Z� ��  � ��$ OpenSans)���  � ��$ 10(���  � ��$ Normal"���  � �� �� " ��.R�� .+�d� $��ToggleButton��$��2��$# Press=�|�.  8��   �� $$ IsDropDownOpen�% �� *TemplatedParent+$& TwoWay=��� $��ContentSite��$��5, 3, 23, 3q�' .  $
��Left=�$��Center=�#��z( #��z) #��z���� $��PART_EditableTextBox��$��3,10,0,0q�#+ z* $y�Hidden=�
������ $��Center=�$
��Left=�$��Center=�|�. .������ " ��.R��� $��PART_ContentHost��|�.  .��� $��Popup��$��Bottom=�|�.  , . #��z- $��Slide=�.��� $��DropDown�� . #��z. #��z/ .+��� $��DropDownBorder��
�������$��
0, 1, 0, 0q�$ 0��$��1,1,1,1q�8�� �� $��4q� . .���� $0 	Contained=� P�  �@��HasItems .  . ��� $ DropDownBorder�� � ��$ 95��P�  � {� .  . ��� 8   � ��P�  �@��
IsGrouping . . ���  � �� .  P�  �@��
IsEditable . . ���  �@ 	IsTabStop .  �� $ PART_EditableTextBox�� � y�$ Visible=��� $ ContentSite�� � y�$ Hidden=�.��� .+�� -   $ grdAddActivity��$��10q�
������� �� $��0 ��� $��0 ��� $��2 ��� $
��Auto ��� $��5 ��� $
��Auto ��� $��10 ��� $��* ��� $��10 ��� $
��Auto ��� $��10 �.+��� $��3��1 �� Orientation$1 
Horizontal=�$��	20 0 0 0 q�.+��� $��OpenSans)�$��12(�$
��Bold"����#666666$��Client: ��#��[ �� -   $ ��AddActivityClientCodeLabel��$��OpenSans)�$��12(�$�� ��#��[ �� -   $ ��AddActivityClientDescLabel��$��OpenSans)�$��20,0,0,0q�$��12(�$�� ��#��[ �� $��5��$1 
Horizontal=�$��	20 0 0 0 q�.+��� $��OpenSans)�$��12(�$
��Bold"����#666666$��Add to ��#��[ �� -   $��AddToActivityTypeComboBox��$	��180��$��30������ .R��� $��OpenSans)�$��12(�$��Normal"�$
��auto������ � $��7��$��	20 0 20 0q� �� $��30 ��� $��* �.+��� $��Stretch=�$��Stretch=����	#FF0073CE._�]� .+��� -   $��tableHeader��$��Center=�$��Account Structure��$��12(�$��OpenSans)�
�������$
��Left=�$��10 2 0 0q��� -   $��includeHistoryorClosed��$��Include history��
�������$��0,2,10,0q�$��Center=�$��11(�$��OpenSans)�$��Center=�$��Right=�� $��1��.+�&   System.Windows.Controls.DataGrid  -   2  Name$2 dgAddActivity��
�������$
��auto��3   RowHeaderWidth$3 0Q�4   
IsReadOnly4 . $��8��5   AutoGenerateColumns5 .  6   GridLinesVisibility$
6 None=����	#FFC8EBFC$��1q�7   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$7 * $��Stretch=�  =  7System.Windows.Controls.Primitives.DataGridColumnHeader)  #System.Windows.Controls.DataGridRow8 �� Triggers*  $System.Windows.Controls.DataGridCell9 �� Padding: � ToolTip��%�  '        '  m     '      �� "  .���  � �� �� "  .R��� #��z��$��OpenSans)�$��12(����#555555���#C6EBFD�� "  8 P�  �@ 
IsSelected . . ���  � ��
 �������  � �� �#8dca35P�  �@ 
IsSelected .  . ���  � �� �#666666�� "  .���  � ��$ Center=���  � �� �� "  .R��� $9 3q�#��z��$��0q�#��z�� . ._��� # z $��Center=����� .R��� $��	mainblock��$��CharacterEllipsis=�$��25��$��Calibri)�$��13(�$��Center=�$��5 5 0 0q����� *Text+: �� *Text+ P�  �@ 
IsSelected . . ���  � ��
 �������  � �� �#8dca35P�  �@ 
IsSelected .  . ���  � �� �#666666�� -	   $��OpenActivityErroLabel��' .  $��No records found��$��Light"�$	��Top=�$��Center=�$��	-2,32,0,0q����	#FF9d9d9d���� " ��.���  � y�$ 	Collapsed=�8 �� ; �� Binding; �� *Items.Count+< �� ElementName$< dgAddActivity��= �� Value= 0.a���  � y�$ Visible=��� $��9��$1 
Horizontal=�$��Right=�$��0 0 20 0q�.+�!  CustomControls.CustomButton  -
   >  Name$> btnAddActivityContinue��$��70��$��30��{�.  ?   BackgroundImage$? ../Asset/continue-btn.png��  -   $> btnAddActivityCancel��$��70��$��30��$��10 0 0 0q�$? ../Asset/cancel-btn.png��