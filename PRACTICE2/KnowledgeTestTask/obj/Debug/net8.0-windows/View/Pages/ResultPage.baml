   M S B A M L   `   `   `  ����L  HKnowledgeTestTask, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:KnowledgeTestTask.View.PagesKnowledgeTestTask.View.Pages  5      I)clr-namespace:KnowledgeTestTask.ViewModelKnowledgeTestTask.ViewModel  5      -    'KnowledgeTestTask.View.Pages.ResultPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:KnowledgeTestTask.View.Pages     5      ?	viewmodel)clr-namespace:KnowledgeTestTask.ViewModel     5      .��5      �� $��Center=�6   $��Center=�5      .+�5   
   �� $%��Результаты теста��6   $��28(�5      $
��Bold"�5      $��0 0 0 30q�5      $��Center=�5      � 5   
     � RowDefinitions  5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� $��0��6   $��20(�5      $��10q�5      �� 5      $#�Всего вопросов: ��6    �� 5       $
��Bold"�5!      �5       �� *TotalQuestions+ �� Mode$ OneWay=�5!      5"      �� 5$      $��1��6   $��20(�5%      $��10q�5&      �� 5'      $+�%Правильных ответов: ��6    �� 5(      $
��Bold"�5)      
��� � ��5(      �� *CorrectAnswers+$ OneWay=�5*      5+      �� 5-      $��2��6   $��24(�5.      $��10q�5/      $
��Bold"�50      �� 51      $�Оценка: ��6    �� 52      
����  ��6   �� *Result+53      54      56      �� 58   
   $��Вернуться��6   $	��200��59      $��40��5:      $��0 30 0 0q�5;      $��16(�5<      ��5=      �� *ReturnCommand+5>      5?      