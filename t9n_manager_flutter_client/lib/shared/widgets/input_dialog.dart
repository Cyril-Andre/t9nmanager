import 'package:flutter/material.dart';

import '../../generated/l10n.dart';

class InputDialogController extends ChangeNotifier {
  String value = "";

  void setValue(String value) {
    this.value = value;
    notifyListeners();
  }
}

class InputDialog extends StatefulWidget {
  const InputDialog(
      {Key? key,
      required this.controller,
      required this.callToActionIcon,
      required this.title,
      required this.message,
      String? this.toolTip,
      required this.callBack,
      this.keyboardType})
      : super(key: key);
  final Icon callToActionIcon;
  final String title;
  final String message;
  final String? toolTip;
  final InputDialogController controller;
  final TextInputType? keyboardType;
  final VoidCallback callBack;
  @override
  State<InputDialog> createState() => _InputDialogState();
}

class _InputDialogState extends State<InputDialog> {
  TextEditingController textFieldController = TextEditingController();
/*
  @override
  void initState() {
    widget.controller.addListener(() {
      widget.controller.value = "";
    });
    super.initState();
  }
*/
  @override
  Widget build(BuildContext context) {
    return IconButton(
      tooltip: widget.toolTip,
      icon: widget.callToActionIcon,
      onPressed: () {
        displayTextInputDialog(widget.title, widget.message, context);
      },
    );
  }

  Future<void> displayTextInputDialog(String title, String message, BuildContext context) async {
    return showDialog(
        context: context,
        builder: (context) {
          return AlertDialog(
            title: Text(title),
            content: TextField(
              keyboardType: (widget.keyboardType ?? widget.keyboardType),
              controller: textFieldController,
              decoration: InputDecoration(hintText: message),
            ),
            actions: <Widget>[
              TextButton(
                child: Text(S.of(context).common_button_cancel),
                onPressed: () {
                  widget.controller.setValue("");
                  Navigator.pop(context);
                },
              ),
              TextButton(
                child: Text(S.of(context).common_button_ok),
                onPressed: () {
                  widget.controller.setValue(textFieldController.text);
                  widget.callBack();
                  Navigator.pop(context);
                },
              ),
            ],
          );
        });
  }
}
