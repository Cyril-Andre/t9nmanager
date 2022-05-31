import 'package:flutter/material.dart';
import 'package:flutter_holo_date_picker/date_picker.dart';
import 'package:flutter_holo_date_picker/i18n/date_picker_i18n.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';

class Datefield extends StatefulWidget {
  const Datefield({ Key? key }) : super(key: key);

  @override
  State<Datefield> createState() => _DatefieldState();
}

class _DatefieldState extends State<Datefield> {
  TextEditingController txtBirthdate = TextEditingController();
  @override
  Widget build(BuildContext context) {
    double width = MediaQuery.of(context).size.width>context.watch<AppState>().screenMaxSize
                                                      ? context.watch<AppState>().screenMaxSize
                                                      : MediaQuery.of(context).size.width;
    return Container(
      alignment: Alignment.center,
      child: Row(mainAxisAlignment: MainAxisAlignment.spaceBetween, children: [
        Expanded(
          child: TextFormField(
            readOnly: true,
            controller: txtBirthdate,
            decoration: const InputDecoration(label: Text('Birthdate')),
          ),
        ),
        SizedBox(
          width: width * 0.2,
          child: IconButton(
              icon: const Icon(Icons.calendar_today),
              onPressed: openDatePicker),
        ),
      ]),
    );
  }
      void openDatePicker() async {
    var datePicked = await DatePicker.showSimpleDatePicker(
      context,
      initialDate: txtBirthdate.text!=''?DateTime.parse(txtBirthdate.text):DateTime(1994),
      firstDate: DateTime(1920),
      lastDate: DateTime.now(),
      dateFormat:  "yyyy-MM-dd",
      locale: DateTimePickerLocale.en_us,
      looping: true,
    );
    if (datePicked != null) {
      txtBirthdate.text =  DateFormat('yyyy-MM-dd').format(datePicked);
    }
  }

}