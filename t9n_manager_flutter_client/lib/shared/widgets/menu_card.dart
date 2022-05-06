import 'package:flutter/material.dart';

class MenuCard extends StatelessWidget {
  const MenuCard({Key? key, required this.cardSize, required this.title, this.onPressed}) : super(key: key);

  final double cardSize;
  final String title;
  final VoidCallback? onPressed;
  @override
  Widget build(BuildContext context) {
    return SizedBox(
        width: cardSize,
        height: cardSize,
        child: Card(
          child: Align(
            alignment: Alignment.center,
            child: ListTile(
                title: Text(title,
                    textAlign: TextAlign.center,
                    style: const TextStyle(fontSize: 12,)
                ),
                onTap: onPressed??onPressed,
            ),
          ),
          elevation: 8,
          shadowColor: Colors.black87,
          margin: const EdgeInsets.all(10),
        )
    );
  }
}
