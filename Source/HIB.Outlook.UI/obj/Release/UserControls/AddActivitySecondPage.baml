   M S B A M L   `   `   `  ���� K  GPeoplePaneRegion, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls  I ECustomControls, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullY=clr-namespace:CustomControls.Controls;assembly=CustomControlsCustomControls.Controls G4clr-namespace:CustomControls;assembly=CustomControlsCustomControls 6    0OutlookAddIn1.UserControls.AddActivitySecondPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:OutlookAddIn1.UserControls     Pcustom=clr-namespace:CustomControls.Controls;assembly=CustomControls     Ccc4clr-namespace:CustomControls;assembly=CustomControls     -   $
��auto��  � 	Resources     ScrollBarTrackThumb �� 
TargetType �� Property �� Value �� 
TargetType �Name ��Name �� CornerRadius �� Triggers	 P� Property
 P� Value �� 
TargetName  TxtBlockActvityStyle � RowDefinitions ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton P� 
SourceName  ButtonSelectBrush  	DarkBrush �� Angle  lblUnboldStyle  ComboBoxNormalBorderBrush �� Color ! ComboBoxNormalBackgroundBrush # ComboBoxDisabledForegroundBrush # ComboBoxDisabledBackgroundBrush 	 ComboBoxDisabledBorderBrush  
 ComboBoxToggleButtonTemplate � ColumnDefinitions �� 	ClickMode d� 	IsChecked �� Path �� RelativeSource �� Mode J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate  �� 
IsReadOnly! �� 
IsReadOnly" � AllowsTransparency# �� IsDropDownOpen$ J� SnapsToDevicePixels% � ActualWidth& �� MaxDropDownHeight-  'System.Windows.Input.KeyboardNavigation'   DirectionalNavigation��%�  &	        &	 �    '�� /    &	 >    &	 �    &	 �    &	 �    &	 �    &		 
    &	
     7  7  7  7  7  7 	 7  '�� �	    7 
 7  7  �� " n�.���  � �� �� " n�.R�� $
 Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��
������ �� $ CornerScrollBarRectangle��$ 5��$��Stretch=�$��Stretch=�$
��Auto��$
��Auto��$��0,1,0,1q�#��z�� P� 	 �@�Tag$
 
Horizontal��. ��� $ CornerScrollBarRectangle�� � ��$
 Auto���� $ CornerScrollBarRectangle�� � ��$ 6���� " ��.���  � ��$ OpenSans)���  � ��$ 12(���  � ��$ SemiBold"���  � �� �#666666��  � ��$ 25����  � ��$ 0 5 0 0q��� " ��.���  �@��IsFlicksEnabled .  ��  � �� �#ADABAB��  � ��
 ���� ��  � ��$ 7����  � �� �� " ��.R�� $ GridRoot��$��7��#��z�� �� $��0.00001* �.+�]� $ 
PART_Track��$��0����. |�.   n� $ Thumb��#��z��#���    �� $ PageUp��$��ScrollBar.PageDownCommand��$ 0Q�|�.   �� $ PageDown��$��ScrollBar.PageUpCommand��$ 0Q�|�.   P� $ Thumb��	 �@J�IsMouseOver
 . . ��� # �  $ Thumb�� � ��P� $ Thumb��	 �@n�
IsDragging
 . . ��� # �  $ Thumb�� � ��P� 	 � {�
 .  . ��� $ Thumb�� � y�$ 	Collapsed=�P� 	 �@��Orientation$
 
Horizontal=�. ��� $ GridRoot�� �@�LayoutTransform �� $	 -90Q��� $ 
PART_Track�� �@�LayoutTransform �� $	 -90Q���  � ��$
 Auto����  � ��$ 8���� $ Thumb�� �@�Tag$ 
Horizontal���� $ PageDown�� � ��$ ScrollBar.PageLeftCommand���� $ PageUp�� � ��$  ScrollBar.PageRightCommand���� " ��.���  � ��$ OpenSans)���  � ��$ 12(���  � ��$ 25����  � ��$ Center=���  � ��$ Normal"��� $ #c6c6c6���� $
 #fff���� $
 #888���� $
 #eee���� $
 #888���� " d�.R��  �� �� $��20 �.+��� $��Border��$��2��8��  $ 0��$��
1, 1, 1, 1q�8�� �� $��ButtonBorder��$��1��$��
1, 1, 1, 1q����#444$ 
0, 0, 0, 0��$��
0, 0, 0, 0q�8�� F� $��Arrow��$��1��=����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	$��Center=����#444$��Center=� P� 	 �@J�IsMouseOver
 . . ��� $ ButtonBorder�� � ��
 �����P� 	 �@d�	IsChecked$

 TrueY�. ��� $ ButtonBorder�� � ��
 ������� $ Arrow�� � �� �	#FF8D979EP� 	 � {�
 .  . ��� $ Border��8   � ���� $ ButtonBorder��8   � ���� $ ButtonBorder��8   � ���� 8   � ���� $ Arrow�� � �� �#999�� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  � ��
 �   ���  �@�FocusVisualStyle Z� ��  � ��$ OpenSans)���  � ��$ 10(���  � ��$ Normal"���  � �� �� " ��.R�� .+�d� $��ToggleButton��$��2��$ Press=�|�.  8��   �� $ IsDropDownOpen� �� *TemplatedParent+$ TwoWay=��� $��ContentSite��$��5, 3, 23, 3q� .  $
��Left=�$��Center=�|�. #��z #��z #��z���� $��PART_EditableTextBox��$��3,0,0,0q�#! z  $y�Hidden=�
������ $��Center=�$
��Left=�$��Center=�|�. .������ " ��.R��� $��PART_ContentHost��|�.  .��� $��Popup��$��Bottom=�|�.  " . #��z# $��Slide=�.��� $��DropDown��$ . #��z% #��z& .+��� $��DropDownBorder��
�������$��
0, 1, 0, 0q�$ 0��$��1,1,1,1q�8�� �� $��4q�$ . .���� $' 	Contained=� P� 	 �@��HasItems
 .  . ��� $ DropDownBorder�� � ��$ 95��P� 	 � {�
 .  . ��� 8   � ��P� 	 �@��
IsGrouping
 . . ���  � �� .  P� 	 �@��
IsEditable
 . . ���  �@ 	IsTabStop .  �� $ PART_EditableTextBox�� � y�$ Visible=��� $ ContentSite�� � y�$ Hidden=�.��� .+��� -   ( ��Name$( scrAddActivity��$��10q�
�������$
��Auto=�$y�Visible=�.��� $��
20 2 20 30q���. 
�������$
��Auto=� �� $��0 ��� $��0 ��� $
��Auto ��� $��4 ��� $
��Auto ��� $��10 ��� $
��Auto ��� $��10 ��� $��* �.+��� ) �� Orientation$) 
Horizontal=�$��2��.+��� $��OpenSans)�$��12(�$
��Bold"����#666666$��Client: ��#��[ �� -   $!��AddActivity2ClientCodeLabel��$��Center=�$��OpenSans)�$��12(�#��[ �� -   $!��AddActivity2ClientDescLabel��$��Center=�$��OpenSans)�$��20,0,0,0q�#��[ �� $��4��$) 
Horizontal=�.+��� $��Calibri)�$
��Bold"����#666666$��14(�$��Attaching to ��$��28��#��[ �� -   $��	addToType��$��Calibri)�$��	-10,0,0,0q�$
��Bold"����#666666$��14(�$��28��#��[ &   System.Windows.Controls.DataGrid  -   *  Name$* SelectedActivityValueGrid����. |�.  +   TabNavigation$
+ None=�$
��Auto=�",   VerticalScrollBarVisibility$, Disabled=�$
��auto��-   
IsReadOnly- . $��6��.   RowHeaderWidth$. 0Q�/   AutoGenerateColumns/ .  0   GridLinesVisibility$
0 None=����	#FFC8EBFC$��1q�1   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$1 * $	��Top=�  )  #System.Windows.Controls.DataGridRow2 �� Triggers=  7System.Windows.Controls.Primitives.DataGridColumnHeader*  $System.Windows.Controls.DataGridCell3 �� Padding4   	IsTabStop5 � ToolTip��%�  '        '  �     '  �     �� "  2 P� 	 �@ 
IsSelected
 . . ���  � ��
 �������  � �� �	#FF3399FFP� 	 �@ 
IsSelected
 .  . ���  � �� �#666666�� "  .���  � �� �� "  .R��� #��z�����#C6EBFD���#3F5B6A�� "  .���  � ��$ Center=���  �@ 	IsTabStop .  ��  � �� �� "  .R��� $3 3q�#��z��$��0q�#��z��$ . ._��� #$ z$ $��Center=����� .R��� $��	mainblock��4 .  $��CharacterEllipsis=�$��25��$��Calibri)�$��13(�$��Center=�$��5 5 0 0q����� *Text+5 �� *Text+ P� 	 �@ 
IsSelected
 . . ���  � ��
 �������  � �� �	#FF3399FFP� 	 �@ 
IsSelected
 .  . ���  � �� �#666666� $��8�� �� $
��Auto ��� $��10 ��� $
��Auto ��� $��10 ��� $
��Auto ��� $��10 ��� $
��Auto ��� $��20 �.+��� $��Stretch=�$��Stretch=����	#FF0073CE$��1q�$ 3��._��  �� $��30 ��� $��* �.+��� $��Stretch=�$��Stretch=����	#FF0073CE._��� $��Calibri)�$
��Bold"�$��12(�$��Add Activity��
�������$��Center=�$
��Left=�$��10 0 0 0q�� $��1��$��20 10 10 10q� �� $��30 ��� $��10 ��� $
��Auto ��� $��10 ��� $��30 � �� $��90 ��� $��* �.+��� $) 
Horizontal=�.+��� $
��Code��#��[ �� $��*��
���  ��$��OpenSans)�$��12(�$
��Bold"�$��0 -4 0 0q��� -   $ brdmainAddActivityCode��
���  ��$��1q�$y�	Collapsed=�$��1��$
��Left=�$	��112���� $��1��$��5��$) 
Horizontal=�.+��� -   $��AddActivityCode��$��1��$��0��$��0,0,0,0q�$	��110��$��OpenSans)�$��12(�$��30��6 �� 
IsEditable6 . 7 �� IsTextSearchEnabled7 .  8 �� IsTextSearchCaseSensitive8 .  9 w� TextPath$
9 Code��: �� StaysOpenOnEdit: . $��Center=�$
��Left=�$��Center=����� .R�� $��Center=� �� $��50 ��� $
��auto ��� $��* �.+��� $��Normal"�$��Center=�$��Stretch=�$
��auto������ $
 Code��� $��1��$��Stretch=�$��1��$��2q�
���   ��� $��2��$��Normal"�$��Center=�$��Stretch=�$
��auto������ $ Description��� -	   $��descriptionTextBlock��$��5,7,0,0q�$��1��$��Center=�$�� �����#333333#��[ �� $) 
Horizontal=�$��0��$��2��$	��Top=�.+��� $��Description��$��0��$��2��$��Center=�#��[ �� $��*��
���  ��$��OpenSans)�$��12(�$
��Bold"�$��0 -4 0 0q��� -
   $��DescriptionTextBox��$��2��; �� 	MaxLength$	; 500��< �� AcceptsReturn< . $��1��$��5��$��0 0 8 0q�$	��500��$��2��$��70��$
��Left=�� $��4��$��0��$��6�� �� $��1* ��� $
��1.2* �.+�� $��0�� �� $��85 ��� $	��200 �.+��� $��0,0,0,0q�$��80��$) 
Horizontal=�$
��Left=�$��0��$��4��$	��Top=�.+��� $��	Who/Owner��$��0��$��2��$��Center=�#��[ �� $��*��
���  ��$��OpenSans)�$��12(�$
��Bold"�$��0 -4 0 0q��� -   $ brdmainownerComboBox��$��5,0,0,0q�
���  ��$	��185��$��1q�$y�	Collapsed=�$��1��$
��Left=�� $��1��= K� ZIndex$= 1��$	��190��$��0,0,0,0q� �� $��80 ��� $	��100 �.+��� -   $��ownerComboBox��$��3��$��0,0,0,0q�> %� IsVirtualizing> . ? %� VirtualizationMode$? 	Recycling=�@ %� IsContainerVirtualizable@ . $��0��$��72��$��OpenSans)�$��12(�$
��Left=�$��4��$��30��$	��Top=�6 . 7 . 8 .  $
9 Code��: . ���� .R�$� ���� .R��  �� $��55 ��� $
��auto ��� $��* �.+��� $��0��$��Normal"�$��Center=�$��Stretch=�$
��auto������ $
 Code��� $��1��$��Stretch=�$��1��$��2q�
���   ��� $��2��$��Normal"�$��Center=�$��Stretch=�$
��auto������ $ Description��� -   $��	ownerName��$��WordEllipsis=�$�� ��$��5,7,0,0q�$��1��$��4��$��Center=�#��[ � $��2��$��40,0,0,0q�$��Stretch=� �� $��50 ��� $��* �.+��� $��Priority��$��7,5,0,0q�$��4��$��Center=�$
��Left=�$��0��#��[ �� -   $��PriorityComboBox��$��4��A �� DisplayMemberPath$A CommonLkpName��$��25,0,0,0q�$��1��$��OpenSans)�$��12(�$��4��$��4��$	��170��$
��Left=�6 . 7 . 8 .  : . ���� " ��.���  � ��$ OpenSans)���  � ��$ 12(���  � ��$ 25����  � ��$ Center=��� $��2��$��Stretch=�$��Stretch=����	#FF0073CE$��1q�$ 3��._��  �� $��30 ��� $��* �.+��� $��Stretch=�$��Stretch=����	#FF0073CE._��� $��Detail��$��Center=�$��Calibri)�$
��Bold"�$��12(�
�������$
��Left=�$��10 0 0 0q�� $��1��$��20 10 10 10q� �� $��30 ��� $��10 ��� $
��Auto � �� $
��Auto ��� $��5 ��� $��* ��� $��0 ��� $
��auto ��� $��0 ��� $
��auto �.+��� $) 
Horizontal=�$��Center=�.+��� $��Update��$��0��$��2��$��Center=�#��[ �� -   $ brdmainupdateComboBox��$��17 0 0 0q�
���  ��$	��175��$��2��$��1q�$y�	Collapsed=�$
��Left=��� -   $��updateComboBox��$��5��$��2��6 . 7 . 8 .  $9 CommonLkpName��: . $��18 0 0 0q�$	��172��$
��Left=�$��OpenSans)�$��12(����� .R��� $��Normal"�$��25��$��Center=�$��Stretch=�$
��auto������ $ CommonLkpName��� $��Reminder��$��0,0,0,0q�$��4��#��[ �� $) 
Horizontal=�$��6��$��15 0 0 0q�.+�(  "System.Windows.Controls.DatePicker  -   B  Name$B reminderDatePicker��$��8��$��0 0 0 0q�$��Center=��� $��at��$��OpenSans)�$��12(����#666666$��4��$��5 9 0 0q�.  (CustomControls.Controls.UcDateTimeUpDown  -   $��reminderTextBox��$��9��$	��110��$��Center=�$��Center=�$��30��$��5 0 0 0q��� $��0��$��2��$	��Top=�#��[ $��0 -1 0 0q��� $�
Follow up/���� �� $) 
Horizontal=�$	��Top=�.+��� $��Start��$��0��$��2��$	��Top=��� $��*��
���  ��$��OpenSans)�$��12(�$
��Bold"�$��0 -4 0 0q� �� $�Start���� $) 
Horizontal=�$��2��$��	0.2,0,0,0q�$��2��.+�  -   $B startdateDateTimePicker��$��6��$��17 0 0 0q�$��Center=��� $��at��$��OpenSans)�$��12(����#666666$��4��$��5 8 0 0q�  -   $��	startTime��$��7��$	��110��$��30��$��Center=�$��Center=�$��4 0 0 0q��� $) 
Horizontal=�$��4��$��2��$	��Top=�.+��� $	��End��$��0��$��2��$��Center=�#��[ �� -   C ��Name$C lblEndDateMandatory��$��*��
���  ��$��OpenSans)�$��12(�$
��Bold"�$��0 -4 0 0q�$y�	Collapsed=��� $) 
Horizontal=�$��6��$��2��$��15 0 0 0q�.+�  -   $B EndDatePicker��$��10��$��0 0 0 0q�$��Center=��� $��at��$��OpenSans)�$��12(����#666666$��4��$��5 8 0 0q�  -   $��endTime��$��11��$	��110��$��Center=�$��Center=�$��30��$��5 0 0 0q��� $��4��$��Stretch=�$��Stretch=����	#FF0073CE$��1q�$ 3��._��  �� $��30 ��� $��* �.+��� $��Stretch=�$��Stretch=����	#FF0073CE._��� $��Who to Contact��$��Center=�$��Calibri)�$
��Bold"�$��12(�
�������$
��Left=�$��10 0 0 0q�� $��1��$��	20 10 5 5q� �� $��30 � �� $
��Auto ��� $��10 ��� $��1.29* ��� $��0 ��� $��70 ��� $��5 ��� $��* �.+��� $
��Name��#��[ �� -   $��contactNameComboBox��$��12��6 . 7 . 8 .  $9 ContactName��$��10(�: . $��2��$��
52.5 0 0 0q�$	��192��$��OpenSans)�$
��Left=����� .R��� $��Normal"�$��Center=�$��Stretch=�$
��auto������ $ ContactName��� $��Contact via��$
��Left=�$��4��$��0,5,0,0q�#��[ � $��6��$��0 0 0 0q� �� $
��auto ��� $
��auto �.+��� -   $��contactModeComboBox��$��13��$��0��6 . 7 . 8 .  $9 ContactType��: . 
���   �$��80��$
��Left=�$��OpenSans)�$��12(����� " ��.���  � ��$ OpenSans)���  � ��$ 12(���  � ��
 �   ����� .R��� $��OpenSans)�$��12(�$��Normal"�$��Center=�$��Stretch=�$
��auto������ $ ContactType��� -   $��contactValueTextblock��$��1��$	��140��$��WordEllipsis=�$��Center=�$��OpenSans)�$��12(����#333333$
��auto��$��10 0 0 0q�5 �� *Text+ �� *Self+�� $��6��$��Stretch=�$��Stretch=����	#FF0073CE$��1q�$ 3��._��  �� $��30 ��� $��* �.+��� $��Stretch=�$��Stretch=����	#FF0073CE._��� $
��Note��$��Calibri)�$��Center=�$
��Bold"�$��12(�
�������$
��Left=�$��10 0 0 0q�� $��1��$��20 10 10 10q� �� $��30 ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $��10 ��� $��* ��� $��20 ��� $
��Auto ��� $��10 ��� $��* �.+��� $��Access level��#��[ �� -   $��accessLevelComboBox��$��14��$��2��$	��170��6 . 7 . 8 .  $9 CommonLkpName��: . $
��Left=�$��OpenSans)�$��12(�$��26 0 0 0q����� " ��.���  � ��$ OpenSans)���  � ��$ 12(����� .R��� $��OpenSans)�$��12(�$��Normal"�$��Center=�$��Stretch=�$
��auto������ $ CommonLkpName��� -   $��accessLevelDescription��$��15��$	; 500��$��2��$��1��$	��490��$��	26 10 0 0q�$��70��$��6��$
��Left=�< . �� -   $#��addActivityRequiredFieldAlert��$��7��
���  ��$!��Please fill required fields��$��OpenSans)�$��10(�$y�	Collapsed=�$
��Left=�$��Center=�$��0 0 0 0q�