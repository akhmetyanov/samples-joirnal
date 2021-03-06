SamplesJournal
=======

Функционал приложения - ведение журнала документации по опробованию ВОР.

Создание шаблона:
- Перейти во вкладку "Шаблоны", создать новый шаблон.
- В шаблоне добавить нужные словари - породы, структуры и т.д.
- Создать поля данных: либо добавлять вручную, либо из файла CSV. В случае из файла CSV, будет прочитана первая строка и на основе этих данных созданы поля.
- Перейти к редактированию полей: указать будет ли поле редактироваться, если идет на редактирование, то нужно указать обработчика поля. Если указать, что поле будет использовано для группировки (например, можно сгруппировать точки по номеру профиля), то будет произведена группировка точек по данному полю, но это поле нельзя будет уже редактировать.
    
После того как составили шаблон, нужно сохранить. При нажатии на кнопку сохранить, выполняется сохранение в базу данных. При нажатии на сохранить в файл, будет выполнено сохранение шаблона в JSON файле в папке download/samplesjournal/template. Дальше этот файл можно использовать для передачи на другие устройства.

Создание файла:
- Перейти на вкладку создания файла
- Выбрать параметр создать новый или загрузить новый. Загрузка из файла подразумевает под собой, что есть ранее сохраненный в JSON формат файл.
- Перейти к редактированию.
- При наличии полей для группировки, сначала будет выводится список уникальных значений по данному полю, после можно провалиться по значению в список объектов и начать редактировать точки. При выборе точки, последовательно по полям отображаются инструменты редактированию в соответствии с выбранным в шаблоне для поля. Когда инструмент дойдет до последнего, при нажатии кнопки далее, окно редактирования точки будет закрыта и выполнен возврат на страницу выбора точек. При выполнении возврата, в скрытых параметра данных строки будет установлена дата редактирования в соответствие с датой на момент. После эта дата попадет в выгрузку CSV.

Файл можно выгрузить в JSON и передать на другое устройство, продолжить редактирование там.
После завершения всего можно файл выгрузить в CSV.