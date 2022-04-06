import 'package:flutter/material.dart';


class ScreenTitle extends StatelessWidget {
  final  String title;
  const ScreenTitle(this.title);

  @override
  Widget build(BuildContext context) => Text(
    title,
    style:const TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
    );
}