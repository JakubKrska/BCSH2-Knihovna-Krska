   M S B A M L   `   `   `  ����  KrskaKnihovna(    "KrskaKnihovna.Views.MainWindowView   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  Library��5   	   $	��700��6   $
��1000��6&   .��5	      � $��10,20,20,20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $	��150 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6!   $��2��61   $��24(�5      $
��Bold"�6"   $��Center=�5      $��Center=�61   $��20q�5       ��5      �� *
LabelTitle+5       �� 5#   
   $��1��6   $��0��6"   $	��Top=�62   $��10q�6J   .+�5$      �� $��	Libraries��6   $��5q�6T   $��40��6_   ��6)   �� *ButtonLibrariesCommand+6_   �� 5%      $��Books��6   $��5q�6L   $��40��6W   ��6%   �� *ButtonBooksCommand+6W   �� 5&      $��	Customers��6   $��5q�6T   $��40��6_   ��6)   �� *ButtonCustomersCommand+6_   �� 5'      $��Loans��6   $��5q�6L   $��40��6W   ��6%   �� *ButtonLoansCommand+6W   5(      � 5+   
   $��1��6   $��1��6   .+�5-      � $��0��6   $��10q�6    $
��Left=�6,   $	��Top=�6G   y�6_   �� *FiltersVisibility+ 5.      �� 5/      $
��Auto �6'   �� 50      $
��Auto �6'   �� 51      $
��Auto �6'   �� 52      $
��Auto �6'   �� 53      $
��Auto �6'   �� 54      $
��Auto �6'   �� 55      $
��Auto �6'   .+�58      �� $��0��6   $��Center=�6,   $��Library:��6G   $��5q�6W   �� 59      $��1��6   $��25��5:   D   $	��100��6Q   $��5q�6]   ��59   +   �� *ComboBoxLibrariesItems+ �� SelectedItem 5:      �� *SelectedLibrary+6]   �� 5<      $��2��6   $��Center=�6,   $��Book:��6G   $��5q�6T   �� 5=      $��3��6   $��25��5>   A   $	��100��6N   $��5q�6Z   ��5=   +   �� *ComboBoxBooksItems+ 5>      �� *SelectedBook+6Z   �� 5@      $��4��6   $��Center=�6,   $��	Customer:��6G   $��5q�6X   �� 5A      $��5��6   $��25��5B   E   $	��100��6R   $��5q�6^   ��5A   +   �� *ComboBoxCustomersItems+ 5B      �� *SelectedCustomer+6^   �� 5D      $��6��6   $��Filter��6)   $	��100��6c   $��5q�6o   $��40��6z   ��6:   �� *ButtonFilterCommand+6z   5E      �� 5H      -   5         ��Name$ listBoxInformace��5H   �   $
��Auto=�6�   $
��Auto=�6�   $��Consolas)�6�   $��14(�6  $��	0,60,0,20q�6   $��Center=�63  $	��800��6P  ��6   �� *ListBoxInformationItems+ 6F   �� *SelectedListBoxItem+ �� Mode$ TwoWay=�6P  5I      �� 5L   
   $��2��6   $��0��6"   $��2��62    �� Orientation$ 
Horizontal=�6F   $��Center=�6_   $��10q�6|   .+�5M      �� $	��Add��6   $��5q�6H   $��90��6S   $��40��6^   ��6#   �� *ButtonAddCommand+6^   �� 5N      $
��Edit��6   $��5q�6J   $��90��6U   $��40��6`   ��6$   �� *ButtonEditCommand+6`   �� 5O      $��Delete��6   $��5q�6N   $��90��6Y   $��40��6d   ��6&   �� *ButtonDeleteCommand+6d   �� 5P      $
��Exit��6   $��5q�6J   $��90��6U   $��40��6`   ��6$   �� *ButtonExitCommand+6`   5Q      5S      5T      