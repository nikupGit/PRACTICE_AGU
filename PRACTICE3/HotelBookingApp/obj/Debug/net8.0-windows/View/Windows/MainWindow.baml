   M S B A M L   `   `   `  ����J  FHotelBookingApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:HotelBookingApp.ViewModelHotelBookingApp.ViewModel  5   	        HotelBookingApp.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6vm'clr-namespace:HotelBookingApp.ViewModel     5   	     � Title$<  6Система бронирования номеров��5   	   $	��650��5   	   $	��900��5   	    � DataContext 5	      0   *HotelBookingApp.ViewModel.BookingViewModel  5
   
   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $)��#Новое бронирование��6   $��0��5      $��0 0 0 10q�5      .��5      � $��10q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $��* �6'    5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5       $
��Auto �6$   �� 5!      $
��Auto �6$   �� 5"      $
��Auto �6$   .+�5%      �� $��ФИО:��6   $��0��5&      $��0��5'      $��Center=�5(      �� 5)      $��0��5*      $��1��5+      $��0 0 0 5q�5,      ��5)      �� *
FullName+ �� UpdateSourceTrigger$ PropertyChanged=�5,      �� 5.      $��Тип номера:��6   $��1��5/      $��0��50      $��Center=�51      �� 52       �� SelectedValuePath$ Content��53      $��1��54      $��1��55      $��0 0 0 5q�56       �� SelectedValue 52      �� *
RoomType+.@�57      �� $��Одноместный��6#   �� 58      $��Двухместный��6#   �� 59      $��Люкс��6#   5:      �� 5<      $��Дата заезда:��6   $��2��5=      $��0��5>      $��Center=�5?      (  "System.Windows.Controls.DatePicker  5@      $��2��5A      $��1��5B      $��0 0 0 5q�5C         SelectedDate 5@      �� *CheckInDate+5C      �� 5E      $��Дата выезда:��6   $��3��5F      $��0��5G      $��Center=�5H        5I      $��3��5J      $��1��5K      $��0 0 0 5q�5L       5I      �� *CheckOutDate+5L      �� 5N      $!��Загрузить фото��5O      $��4��5P      $��0��5Q      $��0 0 5 5q�5R      $	��100��5S      ��5N      �� *UploadImageCommand+5S      �� 5T      $ ��Забронировать��5U      $��4��5V      $��1��5W      $��0 0 0 5q�5X      $
��Left=�5Y      $	��100��5Z      ��5T      �� *BookCommand+5Z      5[      5\      � 5^   
   $��1��6    5_      �� 5`      $��2* �6#   �� 5a      $��* �6#   .+�5d      &   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��5f      $��0 0 5 0q�5g      ��5d      �� *
Bookings+	 �� SelectedItem	 5e      �� *SelectedBooking+
   Columns
 5h      0  *System.Windows.Controls.DataGridTextColumn  5i      ,  &System.Windows.Controls.DataGridColumn   Header$ ФИО��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 5k   )   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5j   )   �� *
FullName+5k   )     5l      $ Тип номера��6)   $ * 5n   )    5m   )   �� *
RoomType+5n   )     5o      $ 
Заезд��6)   $ * 5q   )    5p   )   �� *CheckInDate+ �� StringFormat$ d��5q   )     5r      $ 
Выезд��6)   $ * 5t   )    5s   )   �� *CheckOutDate+$ d��5t   )     5u      $ Стоимость��6)   $ * 5w   )    5v   )   �� *
TotalPrice+$ C��5w   )   5y      �� 5{      $��1��5|      $��Uniform=�5}      $��5 0 0 0q�5~      ��5{      �� *ClientImage+5~      5      �� 5�   
   $��2��6    �� Orientation$ 
Horizontal=�5�      $��Right=�5�      $��0,5,0,0q�5�      .+�5�      �� $��Изменить��5�      $	��100��5�      $��0 0 5 0q�5�      ��5�      �� *EditCommand+5�      �� 5�      $��Удалить��5�      $	��100��5�      ��5�      �� *DeleteCommand+5�      5�      5�      5�      