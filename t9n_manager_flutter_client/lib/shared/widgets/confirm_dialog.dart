import 'dart:ffi';

import 'package:flutter/material.dart';

import '../../generated/l10n.dart';

class ConfirmDialogController extends ChangeNotifier {
  bool confirmed = false;

  void onConfirm() {
    confirmed = true;
    notifyListeners();
  }

  void onCancel() {
    confirmed = false;
  }
}

class ConfirmDialog extends StatefulWidget {
  const ConfirmDialog({Key? key, required this.controller, required this.callToActionIcon, required this.title, required this.message, this.toolTip, required this.callBack})
      : super(key: key);
  final Icon callToActionIcon;
  final String title;
  final String message;
  final String? toolTip;
  final ConfirmDialogController controller;
  final VoidCallback callBack;
  @override
  State<ConfirmDialog> createState() => _ConfirmDialogState();
}

class _ConfirmDialogState extends State<ConfirmDialog> {
  String dialogValue = '';

  @override
  Widget build(BuildContext context) {
    return IconButton(
      tooltip: widget.toolTip,
      icon: widget.callToActionIcon,
      onPressed: () {
        displayConfirmInputDialog(widget.title, widget.message, context);
      },
    );
  }

  Future<void> displayConfirmInputDialog(String title, String message, BuildContext context) async {
    return showDialog(
        context: context,
        builder: (context) {
          return AlertDialog(
            title: Text(title),
            content: Text(message),
            actions: <Widget>[
              TextButton(
                child: Text(S.of(context).common_button_cancel),
                onPressed: () {                  
                    widget.controller.onCancel();
                    Navigator.pop(context);                  
                },
              ),
              TextButton(
                child: Text(S.of(context).common_button_ok),
                onPressed: () {
                    widget.controller.onConfirm();
                    widget.callBack();
                    Navigator.pop(context);
                },
              ),
            ],
          );
        });
  }
}
