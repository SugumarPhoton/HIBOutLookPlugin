   M S B A M L   `   `   `  ���� K  GPeoplePaneRegion, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls  I ECustomControls, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullY=clr-namespace:CustomControls.Controls;assembly=CustomControlsCustomControls.Controls G(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls  8    2OutlookAddIn1.UserControls.MultipleEmailAttachment   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Ivsm9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :local(clr-namespace:OutlookAddIn1.UserControls     Pcustom=clr-namespace:CustomControls.Controls;assembly=CustomControls     8src(clr-namespace:OutlookAddIn1.UserControls     -   $	��950��$	��710��  � 	Resources   �� 
TargetType   rbstyle �� Property �� Value �� 
TargetType �� 
Background �� Bullet A�Name �� EndPoint	 �� 
StartPoint
 ��Name'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� TargetProperty �� 
TargetName R� KeyTime  System.Windows.Visibility   Visible R� Value  DescBorderBrush  
FaderStyle �� 	Resources  Key W� From	 W� To g� Duration g� RepeatBehavior g� AutoReverse �� Triggers P� Property P� Value N� EnterActions  FadeStoryboard  BtnAttachmentOnlyStyle ��Name J� SnapsToDevicePixels �� Triggers  �� 
TargetName # BtnAttachmentAndDeleteOnlyStyle  BtnAttachmentStyle  BtnDeleteAttachmentStyle 	 FieldNameStyle 
 FieldValueStyle  ScrollBarTrackThumb! �Name" �� CornerRadius# � RowDefinitions$ ]�Name% ]� Thumb& n�Name' ]� IncreaseRepeatButton( ��Name) J� Opacity* ]� DecreaseRepeatButton+ P� 
SourceName  ButtonSelectBrush  	DarkBrush, �� Angle��%�  &	        &	 o    &	 �    7  &	 -    &	 �    &	 	    &	 q    &		 �    '	� �    &	
     &	 �    '�� j    �� " 	�.���  � �� �� " 	�.R��� 
 ����  � $��15��$��15��$��0 5 0 0q�.+�A� $ Border��$��1������ $ 0.5,1&�$	 0.5,0&����� $		 0,0&�$	 0,1&���� �� $
 	CheckMark��$7��1/PeoplePaneRegion;component/Asset/IsCheckedX2.png��$
��Fill=�$y�	Collapsed=�$��15��$��15��$��0 -2 0 0q��� $
 UncheckMark��$5��//PeoplePaneRegion;component/Asset/UncheckX2.png��$
��Fill=�$y�	Collapsed=�$��15��$��15��$��0 -2 0 0q�   CommonStates��.   
Normal��  	MouseOver��  Pressed��  Disabled��  CheckStates��.   Checked��. �� .��T� $ (UIElement.Visibility)�$ 	CheckMark��..�i� $ 0��# Z   	Unchecked��. �� .��T� $ (UIElement.Visibility)�$ UncheckMark��..�i� $ 0��# Z   Indeterminate��._��� $��4,0,0,0q�$��Center=�$
��Left=���. ��	#FF8D8A8A���� " �� ���� FadeStoryboard .��W� $ (TextBlock.Opacity)�$ 0X�$ 1X�$ 0:0:0.5D�$ 0:0:5�� .  P�  � y�$ Visible=� �� 8��  �� " ��.���  � ��# � ����  � ��$ 1q���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��#��z��#��z��#��z�� . ._�� .+��� $
 BtnImage��$6��0/PeoplePaneRegion;component/Asset/attach_all.png��$
��Fill=�$y�Visible=��� $
 BtnDisableImage��$��../Asset/attach@2x.png��$
��Fill=�$y�	Collapsed=� P�  �@��IsDefaulted . . ��� $  border�� � y�$ Visible=�P�  �@J�IsMouseOver . P�  � �� . . ��� $  border�� � y�$ Visible=�P�  � {� .  . ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��# � ����  � ��$ 1q���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��#��z��#��z��#��z�� . ._�� .+��� $
 BtnImage��$A��;/PeoplePaneRegion;component/Asset/attach_and_delete_all.png��$
��Fill=�$y�Visible=��� $
 BtnDisableImage��$��../Asset/attach@2x.png��$
��Fill=�$y�	Collapsed=� P�  �@��IsDefaulted . . ��� $  border�� � y�$ Visible=�P�  �@J�IsMouseOver . P�  � �� . . ��� $  border�� � y�$ Visible=�P�  � {� .  . ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��# � ����  � ��$ 1q���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��#��z��#��z��#��z�� . ._�� .+��� $
 BtnImage��$��../Asset/attach@2x.png��$
��Fill=�$y�Visible=��� $
 BtnDisableImage��$��../Asset/attach@2x.png��$
��Fill=�$y�	Collapsed=� P�  �@��IsDefaulted . . ��� $  border�� � y�$ Visible=�P�  �@J�IsMouseOver . P�  � �� . . ��� $  border�� � y�$ Visible=�P�  � {� .  . ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��# � ����  � ��$ 1q���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��#��z��#��z��#��z�� . ._�� .+��� $
 BtnImage��$'��!../Asset/attach-and-delete@2x.png��$
��Fill=�$y�Visible=��� $
 BtnDisableImage��$'��!../Asset/attach-and-delete@2x.png��$
��Fill=�$y�	Collapsed=� P�  �@��IsDefaulted . . ��� $  border�� � y�$ Visible=�P�  �@J�IsMouseOver . P�  � �� . . ��� $  border�� � y�$ Visible=�P�  � {� .  . ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��$ OpenSans)���  � ��$
 Wrap=���  � ��$ SemiBold"���  � ��$
 Left=���  � ��$ Center=���  � ��$ 10(���  � �� �#ffffff�� " 	�.���  � ��$ OpenSans)���  � �� �#333333��  � ��$ 0,0,0,0q���  � ��$ Center=��� " ��.���  � ��$ OpenSans)���  � ��$ Normal"���  � ��$ Stretch=���  � ��$ Center=���  � ��$ 10(���  � �� �#ffffff��  � ��$ WordEllipsis=��� " n�.���  � �� �� " n�.R�� $
! Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��
������ �� $ CornerScrollBarRectangle��$" 5��$��Stretch=�$��Stretch=�$
��Auto��$
��Auto��$��0,1,0,1q�#��z�� P�  �@�Tag$ 
Horizontal��. ��� $  CornerScrollBarRectangle�� � ��$
 Auto���� $  CornerScrollBarRectangle�� � ��$ 6���� " ��.���  �@��IsFlicksEnabled .  ��  � �� �#ADABAB��  � ��
 ���� ��  � ��$ 7����  � �� �� " ��.R�� $! GridRoot��$��7��#��z��# �� $��0.00001* �.+�]� $$ 
PART_Track��$��0����. |�.  % n� $& Thumb��#��z��#���  ' �� $( PageUp��$��ScrollBar.PageDownCommand��$) 0Q�|�.  * �� $( PageDown��$��ScrollBar.PageUpCommand��$) 0Q�|�.   P� $+ Thumb�� �@J�IsMouseOver . . ��� # �  $  Thumb�� � ��P� $+ Thumb�� �@n�
IsDragging . . ��� # �  $  Thumb�� � ��P�  � {� .  . ��� $  Thumb�� � y�$ 	Collapsed=�P�  �@��Orientation$ 
Horizontal=�. ��� $  GridRoot�� �@�LayoutTransform �� $	, -90Q��� $  
PART_Track�� �@�LayoutTransform �� $	, -90Q���  � ��$
 Auto����  � ��$ 8���� $  Thumb�� �@�Tag$ 
Horizontal���� $  PageDown�� � ��$ ScrollBar.PageLeftCommand���� $  PageUp�� � ��$  ScrollBar.PageRightCommand��.��� # �� $	��100 ��� $��* �.+�� $��0�����#0072cd# �� $��10 ��� $��20 ��� $��* �- � ColumnDefinitions- �� $��10 ��� $	��60* ��� $��60 ��� $	��35* �.+�� $��1��$��1��- �� $��* ��� $��* �.+�� -   $��
ClientGrid��$��0��- �� $��80 ��� $��0 ��� $
��auto ��� $��5 ��� $
��auto �.+��� $��0��#��[	 $��Center=�$
��Left=�$��Client: ���� -   $��clientCodeTextBlock��$��2��#��[
 $��	KIMSPA-03��$��2 0 0 0q��� -   $ ��clientDescriptionTextBlock��$��4��#��[
 $	��205��$��Kim's Play Customer��� -   $��activityGrid��$��1��- �� $��60 ��� $��10 ��� $
��auto ��� $��5 ��� $
��auto �.+��� $��0��#��[	 $��Center=�$
��Left=�$��
Activity: ���� -   $��activityCodeTextBlock��$��2��#��[
 $
��1MKT���� -   $��activityDescTextBlock��$��4��#��[
 $	��205��$'��!2017 CKPG Marketing communication��� $��2��$��1��- �� $��70 ��� $��0 ��� $��* �.+��� $��0��#��[	 $	��Top=�$��Stretch=��� $�Attaching To�� �� -   . �Name$. activityType��&   System.Windows.Controls.DataGrid  -	   /  Name$/ SelectedActivityValueGrid��$��12,0,0,0q���.  $��65��$��2��$
��Auto=�"0   VerticalScrollBarVisibility$0 Disabled=�$
��auto��1   
IsReadOnly1 . $��6��2   RowHeaderWidth$2 0Q�3   AutoGenerateColumns3 .  4   GridLinesVisibility$
4 None=����	#FFC8EBFC$��1q�5   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$5 * $	��Top=�  )  #System.Windows.Controls.DataGridRow=  7System.Windows.Controls.Primitives.DataGridColumnHeader*	  $System.Windows.Controls.DataGridCell6 �� Padding7 � ToolTip��%J  '        '  �     '	  A    '��     �� "   P�  �@ 
IsSelected . . ���  � ��
 �������  � �� �	#FF3399FFP�  �@ 
IsSelected .  . ���  � �� �#666666�� "  .���  � �� �� "  .R��� #��z�����#82d5ff$��1,0,0,0q�$
��Bold"�$��10(�$��OpenSans)�$��7q����#c7eafc���#333333�� " 	 .���  � ��$ Center=���  � �� .  ��  � �� �� " 	 .R��� $6 3q�#��z��$��0q�#��z�� . ._��� # z $��Center=����� .R��� $��	mainblock��$��CharacterEllipsis=�$��25��$��OpenSans)�$��10(�$��Center=�$��5 5 0 0q����� *Text+7 �� *Text+ P�  �@	 
IsSelected . . ���  � ��
 �������  � �� �	#FF3399FFP�  �@	 
IsSelected .  . ���  � �� �#666666�� " ��.���  �@��IsFlicksEnabled .  ��  � �� �#ADABAB��  � ��
 ���� ��  � ��$ 7����  � �� �� " ��.R�� $! GridRoot��$��7��#��z��# �� $��0.00001* �.+�]� $$ 
PART_Track��$��0����. |�.  % n� $& Thumb��#��z��#���  ' �� $( PageUp��$��ScrollBar.PageDownCommand��$) 0Q�|�.  * �� $( PageDown��$��ScrollBar.PageUpCommand��$) 0Q�|�.   P� $+ Thumb�� �@J�IsMouseOver . . ��� # �  $  Thumb�� � ��P� $+ Thumb�� �@n�
IsDragging . . ��� # �  $  Thumb�� � ��P�  � {� .  . ��� $  Thumb�� � y�$ 	Collapsed=�P�  �@��Orientation$ 
Horizontal=�. ��� $  GridRoot�� �@�LayoutTransform �� $	, -90Q��� $  
PART_Track�� �@�LayoutTransform �� $	, -90Q���  � ��$
 Auto����  � ��$ 3���� $  Thumb�� �@�Tag$ 
Horizontal���� $  PageDown�� � ��$ ScrollBar.PageLeftCommand���� $  PageUp�� � ��$  ScrollBar.PageRightCommand��� $��3��$��3��$��4��# �� $��30 ��� $��* �.+�� $��1��$��0��$��
10,-15,0,0q�- �� $	��50* ��� $	��50* �# �� $��20 ��� $��20 ��� $��20 ��� $��20 �.+��� $��0��#��[	 $��Center=�$
��Left=�$��Folder: ���� -
   $��mainfolderTextBlock��$��1��#��[
 $��HIB Commercial Lines���� $��0��$��1��#��[	 $��Center=�$
��Left=�$��SubFolder 1: ���� -   $��subfolder1TextBlock��$��1��$��1��#��[
 $��	Marketing���� $��0��$��2��#��[	 $��Center=�$
��Left=�$��SubFolder 2: ���� -   $��subfolder2TextBlock��$��1��$��2��#��[
 $��[none]���� $��0��$��3��#��[	 $��Center=�$
��Left=�$��Client Accessible Until: ���� -   $��clientAccessibleDate��$��1��$��3��#��[
 $��
11/22/2017��� $��1��- �� $	��60* ��� $	��40* �# �� �� $��80 �.+�� -   $��leftPartMainGrid��$��0��# �� $��30 ��� $��* �.+�� $��0�����#dfdfdf- �� $	��50* ��� $	��50* �.+��� $
��Left=�$��10,0,0,0q�$��Center=�$��SemiBold"�$��OpenSans)�$��10(�$��List of selected emails��� $��4��$��1��$��Center=�- �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�� $��0��- �� $��* ��� $��* �.+��� $
��Left=�$��Center=�$��SemiBold"�$��OpenSans)�$��10(�$��Desc:���� $��1��$��*��$��OpenSans)�$��10(�
���  ��$
��Bold"�$��0 0 0 0q�	� -   8 	�Name$8 rbCustom��#��[  $��OpenSans)�$��Center=�$��2,0,0,0q�$��10(�$��1��$��Custom��9 	� 	GroupName$
9 Desc��: d� 	IsChecked$
: TrueY�$
��Left=�$��60��	� -   $8 rbEmailDesc��#��[  $��OpenSans)�$��Center=�$��10(�$��Use Email Subj.��$
9 Desc��$��2��	� -   $8 rbActivityDec��#��[  $��OpenSans)�$��Center=�$��10(�$��Use Activity Desc.��$
9 Desc��$��3��$��6 0 0 0q��� $��1��.��� $��1��    SelectedEmailListItem; �� Path< �� UpdateSourceTrigger  SelectedDescriptionListItem= ��Name> J� IsHitTestVisible? �� RadiusX@ �� RadiusYA ��Name��%�  &	       &	 �    �� " ��.���  � ��$ 3 3 3 3q���  � ��$
 Left=���  � ��$	 Top=���  � ��
 ���� ��  � ��$ 1q���  � �� .  ��  � �� �� " ��.R��� $��
mainBorder�����#c7eafc$�� 0 0 0 1q�$��Stretch=�$��Stretch=�._�� $��mainGrid��$��Stretch=�$��Stretch=�$��50��$��2q�$	��280��- �� $��30 ��� $��* �.+�� $��0��$��Stretch=�$��Stretch=�.+��� $��	mailImage��$	��Top=�$��1,5,0,0q�$��Center=�$��16��$��16������ $; 	ImagePath�$< PropertyChanged=�� $��1��$��Stretch=�$��Stretch=�# �� $��20 ��� $��* �.+��� $��OpenSans)�$��WordEllipsis=�$	��160��$
��Left=�$��SemiBold"�$��10(�$��0,10,0,0q�$��Center=�$��0������ *From+�� $��0��$��	0,10,10,0q�$��OpenSans)�$��SemiBold"�$��10(�$��Center=�$��Right=����� *RecievedDate+�� $��1��$��1��$��WordEllipsis=�$
��Left=�$	��250��$��OpenSans)�$��Normal"�$��10(�$��Center=�$��0 0 0 0q�7 �� *	Subject+���� *	Subject+ P�  �@J�IsMouseOver . . ��� $  mainGrid�� � �� �	#FFBADDE9P�  �@��
IsSelected . . ��� $  mainGrid�� � ��
 ������� $  mainGrid�� � �� �#8dca35�� $  	mailImage�� � ��$' !../Asset/email_image_selected.png��P�  �@��
IsSelected .  . ��� $  mainGrid�� � �� �#333333�� $  	mailImage�� � ��$ ../Asset/email_image.png���� " ��.���  � ��$ 3 3 3 3q���  � ��$
 Left=���  � ��$	 Top=���  � ��
 ���� ��  � ��$ 1q���  � �� .  ��  � �� �� " �� P�  �@��
IsSelected . . ��� $  contentPresenter�� � ��
 �����P�  �@��
IsSelected .  . ���  � �� �#333333.R��� $��
mainBorder�����#e0e0e0$�� 0 0 0 1q�$��Stretch=�$��Stretch=�._�� 
������ $��50��$��2q�.+��� $= 	fillColor��$) 0Q����	#FFBADDE9> .  $? 1��$@ 1���� $= 
fillColor2��$) 0Q����#f6f7fa> .  $? 1��$@ 1���� $A contentPresenter��#��z��#��z��#��z��#��z���� $= FocusVisualElement�����#f6f7fa$��1��$y�	Collapsed=�$? 1��$@ 1��- �� $��* ��� $��* �.+�� $��0��.+��� -   B ��Name$B SelectedMailList����.  $��Disabled=�C �� SelectedIndex$C 0��#��[ $	��300��$y�Visible=�$��1��
������ $��Hidden=�D �� SelectedItemD �� *SelectedItem+E �� ElementName$E SelectedDescMailList��F �� Mode$F TwoWay=�� $��1��.+��     BtnCopyAllStyle��%  &	       �� " ��.���  � ��# � ����  � ��
 ���� ��  � ��$ 1q���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��#��z��$" 1��#��z��#��z�� . ._�� .+��� $
 BtnImage��$6��0/PeoplePaneRegion;component/Asset/applyToAll.png��$
��Fill=�$y�Visible=� P�  �@��IsDefaulted . . ��� $  border�� � y�$ Visible=�P�  �@J�IsMouseOver . P�  � �� . . ��� $  border�� � y�$ Visible=�.+��� -   $B SelectedDescMailList��-
  'System.Windows.Input.KeyboardNavigationG 
  TabNavigation$G Local=�$��Disabled=�#��[ ���#f6f7fa$	��300��$y�Visible=�$��1��
������ $��Hidden=����� H �� DataType"H ��Template��.R�� $��descriptionValueGrid��$	��275��$��54�����#f6f7fa.+�� $��2q�- �� $��35 ��� $��35 ��� $��* ��� $��20 �.+��� $��policyYearTextBlock��$y�Visible=�$
��auto�����#333333$��	5,-7 ,0,0q�$��OpenSans)�$��10(�$��Center=�$��Center=����� *
PolicyYear+�� $��policyTypeTextBlock��$y�Visible=�$
��auto�����#333333$��5,-7,0,0q�$��1��$��OpenSans)�$��10(�$��Center=�$��Center=����� *
PolicyType+�� I ��Name$I txtDesc��-   $��Center=�$��25�����#333333#��[ $��OpenSans)�$��10(�J �� 	MaxLength$	J 100��$��2��$��6��$��
10 -10 0 0q�$	��165��$
��Left=�$��Center=����� *TextBoxValue+$< PropertyChanged=�$F TwoWay=�.���� K ��Name$K 
applyToAll��-   $��3����.  $��Center=�$7 Apply to All��$
��Left=�$��0q�
������ $��10��$��15��$��Search��$��2 -5 0 0q����� L �� BasedOn#L [ " ��.���  � y�$ 	Collapsed=� �� M �� BindingM �� *IsApplyToAllNeedToBeVisible+N �� ValueN True.a���  � y�$ Visible=�� $��2��$��2�����#f6f7fa.+�� $��2�����#f6f7fa$��Right=�.+�� -   $��attachanddeleteonly��$	��170��$��0,0,5,0q�$��Right=�$y�Visible=�$��1��$��2��# �� $
��auto ��� $��30 �- �� �� $��0 ��� .+��� -   $K btnAttachmentOnly��$y�Visible=�{�. #��[ $��50��$��70��$��0��$��0��$��2,5,0,0q�
������ �� -   $ K btnAttachmentAndDeleteOnly��$y�Visible=�{�. #��[ $��50��$��70��$��2��$��2,5,0,0q�
������ � -   $��FailureAlertGrid��$��3��$��Stretch=�$��4��$
��auto��$��Stretch=�$��10,0,0,0q�� $��2�����#f6f7fa$��0,0,0,0q�$��Stretch=�$
��Left=�.+��� -   $K btnBack��$��2��$��	10,15,0,0q�$
��Left=�$	��Top=�$��70��$��22������ .R��� $" 2�����#0072cd._��� $
��Back��$��SemiBold"�
�������$��Center=�$��0,5,0,0q�$��20��$��OpeSans)�$��10(��� -   $��requiredFieldAlert��$��3��
���  ��$!��Please fill required fields��$��OpenSans)�$��10(�$y�	Collapsed=�$
��Left=�$��Center=�$��0 55 0 0q�� -   $��rightPartMainGrid��$��1��# �� $��30 ��� $��* �.+��� ���#c7eafc$��0��$��2��$��2q�� $��0�����#c7eafc.+��� $
��Left=�$��10,0,0,0q�$��Center=����#333333$��Normal"�$��OpenSans)�$��10(�$��Message Preview��� $��1��# �� $	��125 ��� $��* �.+�� $��0��$��1q�- �� $��85 ��� $��* �# �� $��20 ��� $��30 ��� $��20 ��� $
��auto �.+��� -   $&�� MailPreviewRecievedTimeTextBlock��$
��Left=�$��0��$��1��$��5,0,0,0q�$��Center=����#9D6681$��Normal"�$��OpenSans)�$��10(�$��Fri 11/7/2017  1:21 PM���� -   $��MailPreviewFromTextBlock��$��1��$
��Left=�$��1��$��5,0,0,0q�$��Center=����#666666$��Normal"�$��OpenSans)�$��15(�$��	Grant Low���� -   $!��MailPreviewSubjectTextBlock��$��1��$	��290��$��WordEllipsis=�$
��Left=�$��2��$��5,0,0,0q�$��Center=����#444444$��SemiBold"�$��OpenSans)�$��12(�$��Artifact changed���� $��3��$��0��$��3��.+�� $��3��$��0��$��3��$
��Left=�$��0,4,0,0q�- �� $
��auto ��� $��* �.+��� $
��Left=�$��2��$��5,0,0,0q�$��Center=����#666666$��Normal"�$��OpenSans)�$��11(�$	��To ���� -    $��MailPreviewToTextBlock��$
��Left=�$��2��$��1��$��5,0,0,0q�$��Center=����#816666$��Normal"�$��OpenSans)�$��11(�$��	Glportest��� $��4��$��0��$��3��$
��Left=�$��Center=�$��0,10,0,0q�- �� $
��auto ��� $��* �.+��� $
��Left=�$��2��$��5,0,0,0q�$��Center=����#666666$��Normal"�$��OpenSans)�$��11(�$	��Cc ������ " ��.���  � y�$ Visible=� �� M �� $
; Text�$E MailPreviewCcTextBlock��N  .a���  � y�$ 	Collapsed=��� -!   $��MailPreviewCcTextBlock��$
��Left=�$��2��$��1��$��5,0,0,0q�$��Center=����#816666$��Normal"�$��OpenSans)�$��11(�$��	Glportest��� $��0��$��0��$��5��.+��� -"   $
 BtnImage��O 
  	IsTabStopO .  $��5,5,0,0q�$��70��$��70��$	��Top=�$
��Left=�$"��../Asset/outlook_preview.png��$
��Fill=�$y�Visible=�� $��1��$��1q�# �� $��5 ��� $��* �.+��� $y�	Collapsed=�$
��Left=�$��5,0,0,0q�$	��Top=����#A0A0A0$��Normal"�$��OpenSans)�$��12(�$Z��T------------------------------------------------------------------------------------��(  "System.Windows.Controls.WebBrowser  -#   $��
webBrowser��$��1��O .  {�.  > .  