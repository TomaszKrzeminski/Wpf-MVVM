   M S B A M L   `   `   `  ����M  IFriendOrganizer.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:FriendOrganizer.UI.ViewFriendOrganizer.UI.View  5   	   .    (FriendOrganizer.UI.View.FriendDetailView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:FriendOrganizer.UI.View     5   	   .��5
   
   �   � RowDefinitions  5      �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      $
��Auto �6    60   �� 5      6!   �� 5      $
��Auto �6   6,   .+�5      �� $��	FirstName��6   $��
10 10 10 0q�6(   6>   �� 5      $��1��6   $��10 10 10 10q�5   6   ��5   #   �� *Friend.FirstName+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5   M   �� 5      $��2��6   $��LastName��6!   $��
10 10 10 0q�64   6J   �� 5   
   $��3��6   $
��10 0q�5   6   ��5      �� *Friend.LastName+$ TwoWay=�$ PropertyChanged=�5   F   �� 5!      $��4��6   $��Email��6!   $��
10 10 10 0q�61   6G   �� 5"      $��5��6   $
��10 0q�6�   ��6#   �� *Friend.Email+$ TwoWay=� �� TargetNullValue$  ��$ PropertyChanged=�6�   �� 5$   
   $��6��6   $��Favorite Language��6   $��
10 10 10 0q�69   6O   �� 5%   
   $��7��6    �� DisplayMemberPath$ DisplayMember��6N    �� SelectedValuePath$ Id��6p   $
��10 0q�6�   ��6    �� *ProgrammingLanguages+ �� SelectedValue 6�   �� *Friend.FavoriteLanguageId+$ TwoWay=�6�   ]� 5&   
   $��8��6   $��10q�6!   .+�5'      �� $��Right=�6   .+�5(      �� $	��Add��6   $	��125��6'   $��10q�63   ��6?   �� *AddPhoneNumberCommand+6k   �� 5)      $��Remove��6   $	��125��6*   $��10q�66   ��6B   �� *RemovePhoneNumberCommand+6q   5*      &   System.Windows.Controls.DataGrid  5+         AutoGenerateColumns .     RowHeaderWidth$ 0Q�6�   ��6   �� *PhoneNumbers+	 �� SelectedItem	 6<   �� *SelectedPhoneNumber+$ TwoWay=�
   Columns
 5,      0  *System.Windows.Controls.DataGridTextColumn  5-         DataGridColumnStyle1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  6)     DataGridEditingColumnStyle   EditingElementStyle# [ 6]   ,  &System.Windows.Controls.DataGridColumn   Header$ Phone numbers��6�      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6�      Binding 6�   �� *Number+$ TwoWay=�$ PropertyChanged=�6  5/      51      �� 52   
   $��9��6    �� Orientation$ 
Horizontal=�6"   .+�53      �� $	��125��6   $��10q�6!   ��6-   �� *SaveCommand+.��6M   Save6S   �� 54      $	��125��6   $��10q�6!   ��6-   �� *DeleteCommand+.��6O   Delete6W   55      57      58      