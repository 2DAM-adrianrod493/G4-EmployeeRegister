   M S B A M L   `   `   `  ����N  JG4-EmployeeRegister, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:G4_EmployeeRegister.ViewsG4_EmployeeRegister.Views  5   	   S.clr-namespace:G4_EmployeeRegister.UserControls G4_EmployeeRegister.UserControls  5   	   (    "G4_EmployeeRegister.Views.UserView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   9local'clr-namespace:G4_EmployeeRegister.Views     5   	   =uc.clr-namespace:G4_EmployeeRegister.UserControls     5   	     � Title$  UserView��5	   	   $	��500��6   $	��800��6'    � 
ResizeMode$ NoResize=�5
   	   ��5      �� 5   
    �� ImageSource$# ../Resources/Fondos/fondo.jpg��6    j� Stretch$
 Fill=�6A    �� Opacity$	 0.5Q�6P   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   � $��0��6   $��10q�6   $��Center=�6(    � ColumnDefinitions 5      �� 5      $��4* �6#   �� 5      $��* �6#   .+�5!      7   1G4_EmployeeRegister.UserControls.UsersControlador  $
��Left=�6"   $��Center=�6=   �� 5$      -   5         ��Name$ 
BotonSalir��5$      $��1��5%      $��Center=�5&      
������ $��0q�5(      ��5)      �� *VolverALogin+.��5*      �� $!��/Resources/Iconos/salir.png��6   $��38��5+         EstiloImagen#��[  5,      5-      5.      �� 51   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��10q�6X   .+�52      �� $��Center=�53      $��Center=�54        EstiloTextoTitulo#��[ 55      ��52      �� *NombreCompleto+55      �� 56      $��50��6,   $��50��68   $��10,0,0,0q�6C   ��6   �� *Foto+6C   57      � 5:   
   $��2��6   $��10q�6    5;      �� 5<      $��* �6#   �� 5=      $��6* �6#   �� 5>      $��* �6#   �� 5?      $��6* �6#   �� 5@      $��* �6#   .+�5C      �� $��Iniciar Jornada��6     EstilosButton#��[ 5D      $��1��5E      $��	15,0,10,0q�5G      ��5F      �� *IniciarJornadaCommand+5G      �� 5H      $��Finalizar Jornada��6   #��[ 5I      $��3��5J      $��	10,0,15,0q�5L      ��5K      �� *FinalizarJornadaCommand+5L      5M      �� 5P   
   $��3��6   $��10q�6   
�������$��1q�6=   	 �� CornerRadius$	 5��6Q   ._�5Q      &   System.Windows.Controls.DataGrid  -   5        
  Name$
 HistorialFichaje��5Q         AutoGenerateColumns$ False��5R      $��Stretch=�5S      $��Stretch=�5T         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5U         
IsReadOnly$
 True��5V         CanUserAddRows$ False��5X         CanUserDeleteRows$ False��5Y      $
��Auto=�5Z      $��Disabled=�5[      ��5W      �� *
Fichajes+   Columns 5\      0  *System.Windows.Controls.DataGridTextColumn  5]      ,  &System.Windows.Controls.DataGridColumn   Header$ Fecha��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *	FechaHora+  5^      $
 Tipo��6)    67   �� *Tipo+5`      5a      5b      5c      