   M S B A M L   `   `   `  ���� I  ECustomControls, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CustomControls;assembly=CustomControlsCustomControls  K GPeoplePaneRegion, Version=5.5.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls 2   ,OutlookAddIn1.UserControls.FailedAttachments   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Ccc4clr-namespace:CustomControls;assembly=CustomControls     Ivsm9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :local(clr-namespace:OutlookAddIn1.UserControls     -     � 	Resources     
FaderStyle �� 
TargetType �� 	Resources  Key �� TargetProperty W� From	 W� To g� Duration g� RepeatBehavior g� AutoReverse	 �� Triggers
 P� Property P� Value N� EnterActions  FadeStoryboard  BtnRetryStyle �� Property �� Value �� 
TargetType ��Name �� CornerRadius J� SnapsToDevicePixels � ColumnDefinitions ��Name �� Triggers �� 
TargetName  BtnDeleteStyle  Listitem'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard �� 
TargetName R� KeyTime R� Value  System.Windows.Visibility ��Name J� Opacity J� IsHitTestVisible  �� RadiusX! �� RadiusY" ��Name��%�
  &	        7  &	 �     &	 �    &	 \    �� " �� ���� FadeStoryboard .��W� $ (TextBlock.Opacity)�$ 0X�$ 1X�$ 0:0:0.5D�$ 0:0:5�� . 	 P� 
 � y�$ Visible=� �� 8��  �� " ��.���  � ��# � ����  � ��$ 0q���  � ��
 �������  � ��$ OpenSans)���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��$ 2��#��z��#��z��#��z�� . ._��  �� $��* ��� $��20 �.+��� ���#005ea9$��5,0,0,0q�$��OpenSans)�$��10(�$��0��$��Center=�$��Center=�$��Retry���� $ BtnDisableImage��$��../Asset/reset.png��$��Uniform=�$��1��$��0,0,5,0q� P� 
 �@��IsDefaulted . . ��� $ border�� � y�$ Visible=�P� 
 �@J�IsMouseOver . P� 
 � �� . . ��� $ border�� � y�$ Visible=�P� 
 � {� .  . ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��# � ����  � ��$ 0q���  � ��
 �������  � ��$ OpenSans)���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �� " ��.R��� $ border��$ 2��#��z��#��z��#��z�� . ._��  �� $��* ��� $��20 �.+��� ���#005ea9$��5,0,0,0q�$��OpenSans)�$��10(�$��0��$��Center=�$��Center=�$��Delete���� $ BtnDisableImage��$��../Asset/delete-icon.png��$��Uniform=�$��1��$��0,0,5,0q� P� 
 �@��IsDefaulted . . ��� $ border�� � y�$ Visible=�P� 
 �@J�IsMouseOver . P� 
 � �� . . ��� $ border�� � y�$ Visible=�P� 
 � {� .  . ���  �@J�Opacity$	 0.5Q��� " ��.���  � ��$ 3 3 3 3q���  � ��$
 Left=���  � ��$	 Top=���  � ��
 ���� ��  � ��$ 1q���  � �� �� " �� P� 
 �@��
IsSelected . . ��� $ contentPresenter�� � ��
 �����.R�� 
������    CommonStates��.   
Normal��  	MouseOver��. �� .��W� $ 	fillColor��$ Opacity�$ 0D�$	 .35X�  Disabled��. �� .��W� $ contentPresenter��$ Opacity�$ 0D�$	 .55X�  SelectionStates��.   
Unselected��  Selected��. �� .��W� $ 
fillColor2��$ Opacity�$ 0D�$	 .75X�  FocusStates��.   Focused��. �� .��T� $ FocusVisualElement��$ 
Visibility�$ 0D�..�i� $ 0��  	Collapsed=�  	Unfocused��.+��� $ 	fillColor��$ 0Q����	#FFBADDE9 .  $  1��$! 1���� $ 
fillColor2��$ 0Q����	#FF99D047 .  $  1��$! 1���� $" contentPresenter��#��z��#��z��#��z��#��z���� $ FocusVisualElement�����	#FF6DBDD1$��1��$y�	Collapsed=�$  1��$! 1���� ���	#FFC8EBFC$��0 0 0 0q�$��1��$��Bottom=�$��Stretch=�.��� .+��� ���#005ea9$��1q�._�� 
�������# � RowDefinitions# �� $��30 ��� .+��� -   $ 	bdrHeader��._��� $��Failed Attachment List��$��10,8,0,0q�$��SemiBold"�$��OpenSans)�$��15(����#005ea9!   CustomControls.CustomButton  -   $   BackgroundImage$$ ../Asset/close_icon.png��% K� ZIndex$
% 1000��$��24��$��0,0,0,0q�$��24��$	��Top=�$��Right=�� $��1��$��5q� �� $
��1.2* ��� $
��Auto ��� .+�� $��0��# �� $��30 ��� �� $��30 �.+�� ���#0072cd �� $��40 ��� �� $��80 ��� $��90 �.+��� -   $��selectAllDescription��& � Cursor$
& Hand��$��Center=�$��Center=�� $��1�� �� $	��100 ��� $��* �.+��� $��0��$��Client��$��Center=�$��SemiBold"�$��OpenSans)�$��12(�
�������$��0,0,0,0q��� $��1��$��Description��$��Center=�$��SemiBold"�$��OpenSans)�$��12(�
�������$��0,0,0,0q��� -   $��retryAttachment��$
& Hand��$��2��$��5q�#��[ �� -   $��deleteAttachment��$
& Hand��$��3��$��15,5,5,5q�#��[ �� $��1�����	#FFC2E5F5$��1q�._�� .+��� -   ' ��Name$' LstFailedAttachment��( �� SelectionMode$( Multiple=�$
��auto��) �� SelectedIndex$) 0��-  'System.Windows.Input.KeyboardNavigation*   TabNavigation$* Cycle=�
������ #��[ $��Hidden=����� .R��� .+��� $��attachmentdescription��-   $��20��$��5,0,0,0q�$��8,0,5,0q�+ d� 	IsChecked+ �� , �� RelativeSource, �� - �� AncestorType$- ListBoxItemL�. �� Path$. 
IsSelected�.���  �� $	��100 ��� $��* �.+��� $��0��$��0,0,0,0q�$	��Top=����� *ClientEpicCode+�� $��5,0,0,0q�$��1��$��WordEllipsis=�$	��Top=����� *Description+�� -	   $��activeClientErroLabel�� .  $��No records found��$��Light"�$	��Top=�$��Center=�$��	-2,12,0,0q����	#FF9d9d9d���� " ��.���  � y�$ 	Collapsed=�	 �� / �� Binding/ �� *Items.Count+0 �� ElementName$0 LstFailedAttachment��1 �� Value1 0.a���  � y�$ Visible=�� -
   $��FailureAlertGrid��$��2��.+��� -   $��failureTextBlock��$y�	Collapsed=�$��0,5,5,5q�$��Retry is InProgress��
��� � �$��SemiBold"�#��[  �� $��1��$��Center=�$��Stretch=�
�������2 �� ShowsPreview2 . $��5��� -   $��rightPartMainGrid��$��2��$y�	Collapsed=�# �� $
��Auto ��� $
��Auto ��� $��* �.+�� $��0,0,0,2q����#ff0000$��40�� �� $��30 ��� .+��� $��../Asset/alert.png��$��Center=�$��Center=�$��20��$��20���� -   $��errorMessage��$��1��$�� ��$��Center=�$��Normal"�$��OpenSans)�$��12(�
�������� $��1�����#b2b2b2.+�� $��Center=�$��10,5,0,5q�# �� �� .+��� 3 �� Orientation$3 
Horizontal=�.+��� $
��Left=�$��Center=�
�������$��Normal"�$��OpenSans)�$��10(�$��
Client :  ���� -   $��clientLookupCode��$
��Left=�$��Center=�
�������$��Normal"�$��OpenSans)�$��10(�$�� ���� $��1��$3 
Horizontal=�.+��� $��1��$
��Left=�$��Center=�
�������$��Normal"�$��OpenSans)�$��10(�$��Activity :  ���� -   $��activityCode��$��1��$
��Left=�$��Center=�
�������$��Normal"�$��OpenSans)�$��10(�$�� ��� $��2��# �� $	��125 ��� $��* �.+�� $��0��$��1q� �� $��85 ��� $��* �# �� $��20 ��� $��30 ��� $��20 ��� $
��auto �.+��� -   $&�� MailPreviewRecievedTimeTextBlock��$
��Left=�$��0��$��1��$��5,0,0,0q�$��Center=����#9D6681$��Normal"�$��OpenSans)�$��10(�$��Fri 11/7/2017  1:21 PM���� -   $��MailPreviewFromTextBlock��$��1��$
��Left=�$��1��$��5,0,0,0q�$��Center=����#666666$��Normal"�$��OpenSans)�$��15(�$��	Grant Low���� -   $!��MailPreviewSubjectTextBlock��$��1��$	��290��$��WordEllipsis=�$
��Left=�$��2��$��5,0,0,0q�$��Center=����#444444$��SemiBold"�$��OpenSans)�$��12(�$��Artifact changed���� $��3��$��0��$��3��.+�� $��3��$��0��$��3��$
��Left=�$��0,4,0,0q� �� $
��auto ��� $��* �.+��� $
��Left=�$��2��$��5,0,0,0q�$��Center=����#666666$��Normal"�$��OpenSans)�$��11(�$	��To ���� -   $��MailPreviewToTextBlock��$
��Left=�$��2��$��1��$��5,0,0,0q�$��Center=����#816666$��Normal"�$��OpenSans)�$��11(�$��	Glportest��� $��4��$��0��$��3��$
��Left=�$��Center=�$��0,10,0,0q� �� $
��auto ��� $��* �.+��� $
��Left=�$��2��$��5,0,0,0q�$��Center=����#666666$��Normal"�$��OpenSans)�$��11(�$	��Cc ������ " ��.���  � y�$ Visible=�	 �� / �� $
. Text�$0 MailPreviewCcTextBlock��1  .a���  � y�$ 	Collapsed=��� -   $��MailPreviewCcTextBlock��$
��Left=�$��2��$��1��$��5,0,0,0q�$��Center=����#816666$��Normal"�$��OpenSans)�$��11(�$��	Glportest��� $��0��$��0��$��5��.+��� -   $ BtnImage��4   	IsTabStop4 .  $��5,5,0,0q�$��70��$��70��$	��Top=�$
��Left=�$"��../Asset/outlook_preview.png��$
��Fill=�$y�Visible=�� $��1��$��1q�# �� $��5 ��� $��* �.+��� $y�	Collapsed=�$
��Left=�$��5,0,0,0q�$	��Top=����#A0A0A0$��Normal"�$��OpenSans)�$��12(�$Z��T------------------------------------------------------------------------------------��(  "System.Windows.Controls.WebBrowser  -   $��
webBrowser��$��1��4 .  {�.   .  