   M S B A M L   `   `   `  ����M  IFriendOrganizer.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:FriendOrganizer.UI.ViewFriendOrganizer.UI.View  5      P LFriendOrganizer.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\Bclr-namespace:FriendOrganizer.Model;assembly=FriendOrganizer.ModelFriendOrganizer.Model 5      /    )FriendOrganizer.UI.View.MeetingDetailView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:FriendOrganizer.UI.View     5      X TXceed.Wpf.Toolkit, Version=4.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      NmodelBclr-namespace:FriendOrganizer.Model;assembly=FriendOrganizer.Model  5      .��5      �   � RowDefinitions  5   
   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   �� 5      $
��Auto �6   6,   .+�5   
   �� $��Title��6   $��
10 10 10 0q�6    66   �� 5   
   $��1��6   ��6   �� *Meeting.Title+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6p   �� 5   
   $��2��6   $��Title��6   $��
10 10 10 0q�6-   6C   &   Xceed.Wpf.Toolkit.DateTimePicker  5   
   $��3��6   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 6+   �� *Meeting.DateFrom+$ TwoWay=�$ PropertyChanged=�6�   �� 5   
   $��4��6   $��Title��6   $��
10 10 10 0q�6-   6C     5   
   $��5��6    6+   �� *Meeting.DateTo+$ TwoWay=�$ PropertyChanged=�6~   � 5    
   $��6��6   $��10q�6    � 	Resources 5!       �� DataType"  FriendOrganizer.Model.Friend��%�   (��       5"      *, +)�� 6   "  6   .R�5#      �� �� 6!   �6%   �� *	FirstName+ �� 6F   �6J   �� *
LastName+6i   5$      5%       � ColumnDefinitions 5'      �� 5(      6%   �� 5)      $
��Auto �6#   62   �� 5*      6%     5-      �� 5.      $
��Auto �6    60   �� 5/      6"   .+�52      �� $��Added Friends��6   6.   �� 53      $��1��6   ��6$   �� *AddedFriends+ �� SelectedItem 6I   �� *SelectedAddedFriend+6x   �� 54      $��1��6   $��1��6&   .+�55      �� $��<<��6   $��10q�6,   ��68   �� *AddFriendCommand+6_   �� 56      $��>>��6   $��
10 0 10 10q�6,   ��6@   �� *RemoveFriendCommand+6j   57      �� 59      $��Available Friends��6   $��2��60   5;      �� 5=      $��1��6   $��2��6~   ��6$   �� *AvailableFriends+ 6M   �� *SelectedAvailableFriend+6�   5?      �� 5B   
   $��7��6    �� Orientation$ 
Horizontal=�6"   .+�5C      �� $
��Save��6   $��75��6$   $��10q�6/   ��6;   �� *SaveCommand+6]   �� 5D      $��Delete��6   $��75��6&   $��10q�61   ��6=   �� *DeleteCommand+6a   5F      5H      5I      