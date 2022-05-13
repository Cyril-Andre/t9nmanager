import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../../../shared/app_state_notifier.dart';
import '../models/tenant.dart';

class TenantCard extends StatefulWidget {
  const TenantCard({Key? key, this.tenant, required this.addNew}) : super(key: key);
  final Tenant? tenant;
  final bool addNew;

  @override
  State<TenantCard> createState() => _TenantCardState();
}

class _TenantCardState extends State<TenantCard> {
  late Icon icon;

  @override
  Widget build(BuildContext context) {
    if (widget.addNew) {
      icon = const Icon(Icons.add);
    } else {
      icon = const Icon(Icons.delete);
    }
    double cardSize = context.watch<AppState>().cardSize;
    return SizedBox(
      width: cardSize,
      height: cardSize,
      child: Card(
        elevation: 8.0,
        shadowColor: Colors.black87,
        margin: const EdgeInsets.all(8),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Icon(
              Icons.person,
              size: 20.0,
            ),
            Text(
              widget.tenant!.tenantName,
              textAlign: TextAlign.center,
              style: const TextStyle(fontSize: 10),
            ),
            ButtonBar(
              alignment: MainAxisAlignment.end,
              mainAxisSize: MainAxisSize.min,
              buttonMinWidth: 6.0,
              buttonHeight: 6.0,
              buttonPadding: const EdgeInsets.all(1.0),
              children: [
                IconButton(onPressed: () => {}, icon: icon),
              ],
            )
          ],
        ),
      ),
    );
  }
}
