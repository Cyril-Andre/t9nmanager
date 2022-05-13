  import 'package:flutter/material.dart';

alert(String title, String message, String buttonLabel, BuildContext context) {
    return  AlertDialog(
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
            title: Text(title),
            content: Text(message),
            actions: <Widget>[TextButton(onPressed: () => Navigator.pop(context, buttonLabel), child: Text(buttonLabel))]);
  }
  