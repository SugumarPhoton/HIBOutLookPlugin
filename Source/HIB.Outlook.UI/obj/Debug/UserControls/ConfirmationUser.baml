   M S B A M L   `   `   `  ����I  ECustomControls, Version=5.4.0.0, Culture=neutral, PublicKeyToken=nullG4clr-namespace:CustomControls;assembly=CustomControlsCustomControls  5   	   K GPeoplePaneRegion, Version=5.4.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:OutlookAddIn1.UserControlsOutlookAddIn1.UserControls 5      1   +OutlookAddIn1.UserControls.ConfirmationUser   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Ccc4clr-namespace:CustomControls;assembly=CustomControls     5   	   :local(clr-namespace:OutlookAddIn1.UserControls     5        � AllowsTransparency  . $	��150��5	   Q   $	��500��6^    � WindowStartupLocation$ CenterOwner=�6j    � WindowStyle$
 None=�6�   .��5
      � .+�5   
   �� ���#005ea9$��1q�6'   $	��150��6;   $	��500��6H   
������  J� BitmapEffect 5      F� 5       F� Color$ #0072C6��6)    F� 	Direction$	 320Q�69    F� ShadowDepth$ 0Q�5   )    F� Opacity$	 0.1Q�69   ._�5      �  � RowDefinitions 5      �� 5      $��30 �6$   �� 5      $��* �6$   .+�5      ]� $��0��6   $��30��6*   $	��Top=�66   .+�5      !   CustomControls.CustomButton  -   5        	   BackgroundImage$"	 ../Asset/searchbox_clear.png��5   &   
 K� ZIndex$

 1000��6U   $��14��6i   $��0,0,5,0q�6u   $��14��6�   $��Center=�6�   $��Right=�6�   6�   5      � 5      $��1��6    5      �� 5      $��50 �6(   �� 5      $��50 �6(   .+�5      �� $��5,0,0,0q�6    $��Center=�61   $��Center=�6N   $?��9Are you sure you want to delete the selected attachments?��6i   6�   �� 5      $��Center=�6!   $��0,0,0,0q�6>    �� Orientation$ 
Horizontal=�6O   $��1��6h   .+�5      �� -   5         ��Name$ btnDone��5   �   ���#005EA9
�������$��0��6I    � Cursor$
 Hand��6Y   $
��Left=�6}   $��0q�6�   
������ $��60��6�   $��25��6�   $	��Yes��6�   $��5,0,5,0q�6�   6  �� 5       -   5        $ 	btnCancel��5    �   ���#005EA9
�������$��0��6I   $
 Hand��6Y   $
��Left=�6   $��0q�6�   
������ $��60��6�   $��25��6�   $��No��6�   $��5,0,5,0q�6  6  5!      5"      5#      5&      5'      5(      