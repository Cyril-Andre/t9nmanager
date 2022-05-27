import 'package:flutter/material.dart';


class ScreenTitle extends StatelessWidget {
  final  String title;
  const ScreenTitle(this.title, {Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) => Text(
    title,
    style:const TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
    );
}